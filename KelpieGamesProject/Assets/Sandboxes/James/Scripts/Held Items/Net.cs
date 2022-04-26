using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Net : Items
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _shootForce, _moveSpeed;
    [SerializeField] Transform _caughtPoint;
    [SerializeField] float _upTime;
    [SerializeField] MeshRenderer _netMesh1, _netMesh2, _netMeshCaught;

    List<FishFlock> _allFish = new List<FishFlock>();

    public override void UseAbility(Camera cam, RawImage camImage, JamesNamespace.ItemHandler itemHandler)
    {
        _netMeshCaught.enabled = false;
        var offset = new Vector3(0, 0.2f, 0); // Intended to tilt Net when shot, remove if inadequate
        var me = Instantiate(this, cam.transform.position + cam.transform.forward * 2, cam.transform.rotation);
        me.GetComponent<Rigidbody>().AddForce((cam.transform.forward + offset) * _shootForce, ForceMode.Force);
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

        if (_rb.velocity.magnitude > _moveSpeed) _rb.velocity = _rb.velocity.normalized * _moveSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FishFlock fish))
        {
            if (fish.State == FishState.Caught) return;

            _netMeshCaught.enabled = true;
            _netMesh1.enabled = false;
            _netMesh2.enabled = false;

            fish.ChangeFishState(FishState.Caught);
            fish.transform.parent = _caughtPoint;
            fish.transform.position = _caughtPoint.transform.position;
            fish.transform.rotation = _caughtPoint.transform.rotation;
            _allFish.Add(fish);
            fish.canMove = false;
        }
    }
}
