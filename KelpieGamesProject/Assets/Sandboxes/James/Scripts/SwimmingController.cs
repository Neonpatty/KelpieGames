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
            VerticalSwim();
            DirectionalSwim();
            SlowingMovement();
        }

        void SlowingMovement()
        {
            _rb.velocity = _rb.velocity * 0.95f;
        }


        void DirectionalSwim()
        {
            var inputX = Input.GetAxisRaw("Horizontal");
            var inputY = Input.GetAxisRaw("Vertical");

            _rb.AddForce(transform.right * inputX * swimSpeed * 50 * Time.deltaTime);
            _rb.AddForce(transform.forward * inputY * swimSpeed * 50 * Time.deltaTime);

            _rb.ClampRBSpeed(_maxSwimSpeed);
        }

        void VerticalSwim()
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

