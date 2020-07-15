using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _angelViewMinX;
    [SerializeField] private float _angelViewMaxX;
    [SerializeField] private float _angelViewMinY;
    [SerializeField] private float _angelViewMaxY;
    [SerializeField] private TouchField _touchField;
    [SerializeField] private GameObject _character;

    public bool _isMobile; // Мобильная версия или десктоп 

    private float _sensitivity;
    private float _mouseXRotation;
    private float _mouseYRotation;
    private Quaternion _nowRotation;
    private Quaternion _nowRotationX;
    private Quaternion _nowRotationY;


    private void Start()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;

        if (PlayerPrefs.HasKey("Sensivity"))
            _sensitivity = PlayerPrefs.GetFloat("Sensivity");
        else
            _sensitivity = 0.5f;

        _nowRotationX = _character.transform.localRotation;
        _nowRotationY = transform.localRotation;
        _nowRotation = _character.transform.localRotation;

    }

    public float ClampAngle(float angle , float min , float max)
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

            _mouseXRotation = _touchField.TouchDist.x * _sensitivity;
            _mouseYRotation = _touchField.TouchDist.y * _sensitivity;

            ClampAngle(_mouseXRotation, _angelViewMinX, _angelViewMaxX);
            _mouseYRotation = Mathf.Clamp(_mouseYRotation, _angelViewMinY, _angelViewMaxY);

            _nowRotationX *= Quaternion.Euler(0f, _mouseXRotation, 0f);
            _nowRotationY *= Quaternion.Euler(-_mouseYRotation, 0f, 0f);

            _character.transform.localRotation = _nowRotationX;
            transform.localRotation = _nowRotationY;

        }
        else
        {

            _mouseXRotation -= Input.GetAxis("Mouse Y") * _sensitivity;
            _mouseYRotation += Input.GetAxis("Mouse X") * _sensitivity;

            _nowRotation = Quaternion.Euler(_mouseXRotation, _mouseYRotation, 0);

            _character.transform.localRotation = _nowRotation;

        }
    }

}
