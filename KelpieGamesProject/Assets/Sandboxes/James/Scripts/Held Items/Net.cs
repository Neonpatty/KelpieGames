using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : Items
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float ShootForce;
    [SerializeField] Transform _caughtPoint;

    public override void UseAbility(Transform cam)
    {
        var me = Instantiate(this, cam.position + cam.forward * 2, cam.rotation);
        me.GetComponent<Rigidbody>().AddForce(cam.forward * ShootForce, ForceMode.Force);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Environment"))
        {
            //_rb.velocity = Vector3.zero;
            
        }
    }
}
