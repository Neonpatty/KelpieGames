using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishState
{
    Swimming = 0, 
    Eating = 1,
    Caught = 2,
}

public class FishFlock : MonoBehaviour
{
    public float speedMax;
    public float speedMin;
    public float speed;
    private FishState _state;
    public Bait FoundBait = null;

    [Tooltip("How fast the fish rotate")]
    float rotationSpeed = 4.0f; //turning speed

    [Tooltip("Fish closer than this distance will flock together, otherwise they will go off by themselves")]
    public float neighbourDistance = 1;

    bool turning = false; //turn around when at the edge of play area.
    Vector3 turnDir;

    // Start is called before the first frame update
    void Start()
    {
        turnDir = GlobalFlock.Instance.transform.position;
        speed = Random.Range(speedMin, speedMax);
        Application.targetFrameRate = 300;
    }

    // Update is called once per frame
    void LateUpdate() //changing this to late update will ensure smoother movement, but also can lead to more CPU usage (as it is occouring at the frame rate, rather than 50x a second. Consider changing this back to fixed update if things dont work out
    {
        switch (_state)
        {
            case FishState.Swimming:
                AimlessSwimming();
                break;
            case FishState.Eating:
                MovingToEat();
                break;
            case FishState.Caught:
                break;
        }
    }

    public void ChangeFishState(FishState newState)
    {
        _state = newState;
    }

    void MovingToEat()
    {
        if (!FoundBait)
        {
            ChangeFishState(FishState.Swimming);
            return;
        }
        var baitPos = FoundBait.transform.position;
        var direction = baitPos - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(baitPos, transform.position) < 0.5f)
        {
            if (!FoundBait.Fishes.Contains(this)) FoundBait.Fishes.Add(this);
        }
        else
        {
            transform.Translate(0, 0, Time.deltaTime * speed);
        }
    }

    void AimlessSwimming()
    {
        if (Random.Range(0, 20) < 1)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5, LayerMask.GetMask("Environment")))
            {
                turning = true;

                var newHitPos = new Vector3(hit.point.x, hit.point.y + (Random.Range(-10, 10)) / 20, hit.point.z);

                turnDir = (newHitPos - transform.position) * -1;
            }
            else if (Vector3.Distance(transform.position, Vector3.zero) >= GlobalFlock.Instance.tankSizeX)
            {
                turning = true;
                var originPos = GlobalFlock.Instance.transform.position;
                turnDir = originPos - transform.position;
            }
            else
                turning = false;
            
        }

        if (turning) //"Turning" ensures that fish don't go outside the play area (defined in GlobalFlock.cs). If you want fish to swim forever, disable this.
        {
            transform.rotation = TurnInDirection(turnDir);
            speed = Random.Range(speedMin, speedMax);
        }
        else
        {
            if (Random.Range(0, 10) < 1) //10% chance of running logic per frame (will run every 10 frames * 0.02s (fixed update) = 0.2s per update), use this to ensure optimisation + more natural behaviour
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    Quaternion TurnInDirection(Vector3 direction)
    {
        
        
        return Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
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
