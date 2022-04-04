using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesNamespace
{
    public class SwimmingController : MonoBehaviour
    {
        [SerializeField] float _maxSwimSpeed, swimSpeed;
        [SerializeField] Rigidbody _rb;

        void FixedUpdate()
        {
            SwimVertical();
            DirectionalSwim();
            SlowingMovement();
        }

        void SlowingMovement()
        {

        }


        void DirectionalSwim()
        {
            var inputX = Input.GetAxisRaw("Horizontal");
            var inputY = Input.GetAxisRaw("Vertical");

            _rb.AddForce(transform.right * inputX * swimSpeed * 50 * Time.deltaTime);
            _rb.AddForce(transform.forward * inputY * swimSpeed * 50 * Time.deltaTime);

            _rb.ClampRBSpeed(_maxSwimSpeed);
        }

        void SwimVertical()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _rb.AddForce(Vector3.up * swimSpeed * 50 * Time.deltaTime);

                _rb.ClampRBSpeed(_maxSwimSpeed);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _rb.AddForce(Vector3.down * swimSpeed * 50 * Time.deltaTime);

                _rb.ClampRBSpeed(_maxSwimSpeed);
            }
        }
    }
}

