using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamController : MonoBehaviour
{
    [SerializeField]
    float mouseSensitivity = 100f;
    [SerializeField]
    Transform playerBody;

    float _xRotation = 0f;
    float _yRotation = 0f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        //Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        //Player Rotation
        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
