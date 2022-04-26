using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesNamespace
{
    public class SwimmingController : MonoBehaviour
    {
        [SerializeField] float _maxSwimSpeed, _swimSpeed;
        [SerializeField] Rigidbody _rb;
        [SerializeField] Transform cameraGameObject;
        
        void FixedUpdate()
        {
            VerticalSwim();
            DirectionalSwim();
            SlowingMovement();
        }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }


        void SlowingMovement()
        {
            _rb.velocity = _rb.velocity * 0.95f;
        }


        void DirectionalSwim()
        {
            var inputX = Input.GetAxisRaw("Horizontal");
            var inputY = Input.GetAxisRaw("Vertical");

            _rb.AddForce(cameraGameObject.right * inputX * _swimSpeed * 50 * Time.deltaTime);
            _rb.AddForce(cameraGameObject.forward * inputY * _swimSpeed * 50 * Time.deltaTime);

            _rb.ClampRBSpeed(_maxSwimSpeed);
        }

        void VerticalSwim()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _rb.AddForce(Vector3.up * _swimSpeed * 50 * Time.deltaTime);

                _rb.ClampRBSpeed(_maxSwimSpeed);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _rb.AddForce(Vector3.down * _swimSpeed * 50 * Time.deltaTime);

                _rb.ClampRBSpeed(_maxSwimSpeed);
            }
        }

        public void MultiplySpeed(int multiplier)
        {
            _swimSpeed *= multiplier;
            _maxSwimSpeed *= multiplier;
        }

        public void DivideSpeed(int multiplier)
        {
            _swimSpeed /= multiplier;
            _maxSwimSpeed /= multiplier;
        }
        
    }
}

