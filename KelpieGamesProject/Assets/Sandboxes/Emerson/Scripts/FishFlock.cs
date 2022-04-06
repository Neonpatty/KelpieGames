using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlock : MonoBehaviour
{
    public float speedMax;
    public float speedMin;
    public float speed;

    [Tooltip("How fast the fish rotate")]
    float rotationSpeed = 4.0f; //turning speed

    [Tooltip("Fish closer than this distance will flock together, otherwise they will go off by themselves")]
    public float neighbourDistance = 1;

    bool turning = false; //turn around when at the edge of play area.

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMin, speedMax);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Random.Range(0,20) < 1) 
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5, LayerMask.GetMask("Enviroment")))
                turning = true;
            else
                turning = false;
            /*
            if (Vector3.Distance(transform.position, Vector3.zero) >= GlobalFlock.Instance.tankSizeX)
                turning = true;
            else
                turning = false;
            */
        }
        
        if (turning) //"Turning" ensures that fish don't go outside the play area (defined in GlobalFlock.cs). If you want fish to swim forever, disable this.
        {
            Vector3 direction = new Vector3(0, 30, 0) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
            speed = Random.Range(speedMin, speedMax);
        }
        else
        {
            if (Random.Range(0, 10) < 1) //10% chance of running logic per frame (will run every 10 frames * 0.02s (fixed update) = 0.2s per update), use this to ensure optimisation + more natural behaviour
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        FishFlock[] fishes;
        fishes = GlobalFlock.Instance.allFish;

        Vector3 vcentre = Vector3.zero; //center of group
        Vector3 vavoid = Vector3.zero; //points away from fish neighbours
        float gSpeed = 0.1f;

        Vector3 goalPos = GlobalFlock.Instance.goalPos;

        float dist;

        int groupSize = 0;
        foreach (FishFlock fish in fishes)
        {
            if (fish == null) continue;

            if (fish == this) continue;

            dist = Vector3.Distance(fish.transform.position, this.transform.position);
            if (dist <= neighbourDistance)
            {
                vcentre += fish.transform.position;
                groupSize++;

                if (dist < 1.0f) //Change this to change how close fish can be before avoidance
                {
                    vavoid = vavoid + (transform.position - fish.transform.position);
                    //We are about to collide with another fish, calculate vector to avoid this fish        
                }
                FishFlock anotherFlock = fish.GetComponent<FishFlock>();
                gSpeed = Mathf.Clamp(gSpeed + anotherFlock.speed, 0.25f, 4); //when in a flock, speed up. Clamp this more to reduce max speed
            }
                
            
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - transform.position);
            speed = Mathf.Clamp(gSpeed / groupSize, 1, 10); //speed will change based on how many people there are in the group

            Vector3 direction = (vcentre + vavoid) - transform.position; //get direction to swim in (center + avoid - position)
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
    }
}
