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
        float _yRotation = 0f;

        [SerializeField] private float _smoothTime;
        private float _verOld;
        private float _vertAngularVelocity;
        private float _horiAngularVelocity;


        void Awake()
        {
            
            Cursor.lockState = CursorLockMode.Locked;
            Application.targetFrameRate = 500;
            _horiHelper.localRotation = transform.localRotation;

            //StartCoroutine(PostSimulationUpdate());
        }

        IEnumerator PostSimulationUpdate()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                yield return waitForFixedUpdate;

                Rotate();
            }
        }

        void Rotate()
        {
            _xRotation -= GetVerticalValue();
            _xRotation = _xRotation <= -90 ? -90 :
                  _xRotation >= 90 ? 90 :
                  _xRotation;

            _yRotation -= GetHorizontalValue();
            _yRotation = _yRotation <= -180 ? -180 :
                  _yRotation >= 180 ? 180 :
                  _yRotation;

            _verOld = _xRotation;
            RotateVertical();
            RotateHorizontal();

        }

        void LateUpdate()
        {
           Rotate();
        }

        protected float GetVerticalValue() => Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        protected float GetHorizontalValue() => Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        protected virtual void RotateVertical()
        {
           // DampVertical();
            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        }

        protected virtual void RotateHorizontal() 
        {

            //DampHoriztonal();
            //transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            // _player.Rotate(Vector3.up, GetHorizontalValue());
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
