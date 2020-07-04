using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[AddComponentMenu("Camera-Control/MouseLook")]
[RequireComponent(typeof(Rigidbody))]
public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _sensivity;
    [SerializeField] private float _angelViewMinX;
    [SerializeField] private float _angelViewMaxX;
    [SerializeField] private float _angelViewMinY;
    [SerializeField] private float _angelViewMaxY;
    [SerializeField] private TouchPad _touchPad;

    public bool _isMobile;
    // Угол врощения в данный отрезок времени

    private float _mouseXRotation;
    private float _mouseYRotation;
    private Quaternion _nowRotation;

    private void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        _nowRotation = transform.localRotation;

    }

    public static float ClampAngle(float angle , float min , float max)
    {
        if(angle < -360f)
            angle += 360f;
        if (angle > 360F)
            angle -= 360f;

        return Mathf.Clamp(angle, min, max);
    }


    private void Update()
    {
        if (_isMobile)
        {
            _mouseXRotation -= _touchPad.VerticalPosition * _sensivity;
            _mouseYRotation += _touchPad.HorizontalPosition * _sensivity;
        }
        else
        {
            _mouseXRotation -= Input.GetAxis("Mouse Y") * _sensivity;
            _mouseYRotation += Input.GetAxis("Mouse X") * _sensivity;



        }
        _mouseXRotation = Mathf.Clamp(_mouseXRotation, _angelViewMinY, _angelViewMaxY);

        Quaternion quat = Quaternion.Euler(_mouseXRotation, _mouseYRotation, 0);
        transform.rotation = quat;
        

    }

}
