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
    public string fishName;
    public SpawnWithinBox OriginRef { get; private set; }
    public GlobalFlock ManagerRef { get; private set; }

    public float speedMax;
    public float speedMin;
    public float speed;
    public FishState State { get; private set; }
    public Bait FoundBait = null;

    public bool canMove; //override fish movement
    [Tooltip("How fast the fish rotate")]
    float rotationSpeed = 6.0f; //turning speed

    [Tooltip("Fish closer than this distance will flock together, otherwise they will go off by themselves")]
    public float neighbourDistance = 1;

    bool turning = false; //turn around when at the edge of play area.
    Vector3 turnDir;

    // Start is called before the first frame update
    void Start()
    {
        //ManagerRef = GameObject.FindObjectOfType<GlobalFlock>();
        
        speed = Random.Range(speedMin, speedMax);
        Application.targetFrameRate = 300;
    }

    // Update is called once per frame
    void FixedUpdate() //Logic runs in fixed update, movement runs in late update = silky smooth at low CPU cost
    {
        switch (State)
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
    private void LateUpdate()
    {
        if(canMove)
            transform.Translate(0, 0, Time.deltaTime * speed);
    }

    public void ChangeFishState(FishState newState)
    {
        State = newState;
    }

    public void SetOrigin(SpawnWithinBox origin)
    {
        OriginRef = origin;
        turnDir = OriginRef.transform.position;
    }
    public void SetManager(GlobalFlock origin)
    {
        ManagerRef = origin;
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
                Quaternion.LookRotation(direction).SetZRotation(0),
                rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(baitPos, transform.position) < 0.15f)
        {
            if (!FoundBait.Fishes.Contains(this)) FoundBait.Fishes.Add(this);
            canMove = false;
        }
        else
        {
            canMove = true;
            //transform.Translate(0, 0, Time.deltaTime * speed);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }

    void AimlessSwimming()
    {
        canMove = true;

        if (Random.Range(0, 15) < 1)
        {
            RaycastHit hit;
            var originDist = Vector3.Distance(transform.position, OriginRef.transform.position);
            var origScale = OriginRef.transform.localScale;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5, LayerMask.GetMask("Environment")))
            {
                turning = true;

                var newHitPos = new Vector3(hit.point.x, hit.point.y + (Random.Range(-10, 10)) / 20, hit.point.z);

                turnDir = (newHitPos - transform.position) * -1;
            }
            else if (originDist >= origScale.x||
                     originDist >= origScale.y||
                     originDist >= origScale.z)
            {
                turning = true;
                //var originPos = OriginRef.transform.position;
                var originPos = OriginRef.RandomPosInCube();
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
       // transform.Translate(0, 0, Time.deltaTime * speed);
    }

    Quaternion TurnInDirection(Vector3 direction)
    {
        return Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction).SetZRotation(0),
                rotationSpeed * Time.deltaTime);
    }

    void ApplyRules()
    {
        List<FishFlock> fishes;
        fishes = ManagerRef.allFish;

        Vector3 vcentre = Vector3.zero; //center of group
        Vector3 vavoid = Vector3.zero; //points away from fish neighbours
        float gSpeed = 0.1f;

        Vector3 goalPos = ManagerRef.goalPos;

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
