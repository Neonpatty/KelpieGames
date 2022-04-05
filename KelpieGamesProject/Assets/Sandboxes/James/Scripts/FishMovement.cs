using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesNamespace
{
    public class FishMovement : MonoBehaviour
    {
        [SerializeField] float _swimSpeed;
        [SerializeField] Rigidbody _rb;
        public Bait FoundBait;
        void FixedUpdate()
        {
            
            var movePos = transform.forward * _swimSpeed * Time.deltaTime;
            var moveRot = transform.rotation;
            transform.position += movePos;
            

            //_rb.AddRelativeForce(transform.forward * _swimSpeed * Time.deltaTime);

            /*
            if (!FoundBait) return;

            if (FoundBait.transform.position.magnitude - transform.position.magnitude < 0.5f)
                transform.position -= movePos;
            else
            {
                if (!FoundBait.Fishes.Contains(this))
                {
                    FoundBait.Fishes.Add(this);
                }
            }
            */
        }

        float FindRandDirection()
        {
            var rand = Random.Range(-36f, 36f);
            var sine = Mathf.Sin(rand);
            Debug.Log(sine);
            return sine;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Boundary"))
            {
                Debug.Log("Hit Boundary");
                transform.forward = -transform.forward;
            }
            else if (col.CompareTag("Bait"))
            {
                FoundBait = col.GetComponent<Bait>();
                transform.LookAt(FoundBait.transform.position);

            }
        }


    }

}
