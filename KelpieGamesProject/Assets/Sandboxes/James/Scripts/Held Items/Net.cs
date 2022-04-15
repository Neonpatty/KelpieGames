using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : Items
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _shootForce;
    [SerializeField] Transform _caughtPoint;
    [SerializeField] float _upTime;
    List<FishFlock> _allFish = new List<FishFlock>();

    public override void UseAbility(Transform cam)
    {
        var me = Instantiate(this, cam.position + cam.forward * 2, cam.rotation);
        me.GetComponent<Rigidbody>().AddForce(cam.forward * _shootForce, ForceMode.Force);
    }

    void Update()
    {
        _upTime -= Time.deltaTime;
        if (_upTime <= 0)
        {

            foreach (var fish in _allFish)
            {
                fish.transform.parent = null;
                fish.transform.position += new Vector3(0, 1, 0);
                fish.transform.rotation = new Quaternion(0, 0, 0, 0);
                fish.ChangeFishState(FishState.Swimming);

            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FishFlock fish))
        {
            fish.ChangeFishState(FishState.Caught);
            fish.transform.parent = _caughtPoint;
            fish.transform.position = _caughtPoint.transform.position;
            fish.transform.rotation = _caughtPoint.transform.rotation;
            _allFish.Add(fish);
            fish.canMove = false;
        }
        else if (other.gameObject.layer == LayerMask.GetMask("Environment"))
        {
            Destroy(_rb);
        }
    }

    Quaternion SetZRot(Quaternion oldRot, int newZ)
    {
        return new Quaternion(oldRot.x, oldRot.y, newZ, oldRot.w);
    }
}
