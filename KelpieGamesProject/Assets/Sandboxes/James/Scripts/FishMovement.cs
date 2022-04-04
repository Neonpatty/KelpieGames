using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float swimSpeed;
    [SerializeField] LayerMask colMask;
    void Update()
    {
        var movePos = transform.forward * swimSpeed * Time.deltaTime;
        var moveRot = transform.rotation;
        transform.position += movePos;
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
            
        
    }


}
