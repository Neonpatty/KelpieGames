using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesNamespace
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] float MouseSensitivity = 100f;
        [SerializeField] Transform _player;
        [SerializeField] Transform _horiHelper;
        float _xRotation = 0f;

        [SerializeField] private float _smoothTime;
        private float _verOld;
        private float _vertAngularVelocity;
        private float _horiAngularVelocity;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _horiHelper.localRotation = transform.localRotation;
        }

        void Rotate()
        {
            _xRotation -= GetVerticalValue();
            _xRotation = _xRotation <= -90 ? -90 :
                  _xRotation >= 90 ? 90 :
                  _xRotation;

            _verOld = _xRotation;
            RotateVertical();
            RotateHorizontal();

        }

        void Update()
        {
            Rotate();
        }

        protected float GetVerticalValue() => Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        protected float GetHorizontalValue() => Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        protected virtual void RotateVertical()
        {
            DampVertical();
            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        }

        protected virtual void RotateHorizontal() 
        {
            DampHoriztonal();
            _player.Rotate(Vector3.up, GetHorizontalValue());

            
        }
    

        void DampHoriztonal()
        {
            _horiHelper.Rotate(Vector3.up, GetHorizontalValue(), Space.Self);
            _player.localRotation = Quaternion.Euler(
                0f,
                Mathf.SmoothDampAngle(
                    _player.localEulerAngles.y,
                    _horiHelper.localEulerAngles.y,
                    ref _horiAngularVelocity,
                    _smoothTime),
                0f);
        }
        void DampVertical()
        {
            _xRotation = Mathf.SmoothDampAngle(_verOld, _xRotation, ref _vertAngularVelocity, _smoothTime);
        }

    }
}
