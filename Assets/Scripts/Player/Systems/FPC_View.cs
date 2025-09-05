using System;
using UnityEngine;

public class FPC_View : MonoBehaviour
{
    [SerializeField]
    private float sensivityX = 8;
    [SerializeField]
    private float sensivityY = 0.5f;
    [SerializeField]
    private Transform cameraPoint;
    [SerializeField, Range(0, 90)]
    private float verticalClamp = 90;

    private float x, y;
    private float xRotation = 0;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReceiveInput(Vector2 input)
    {
        x = input.x; 
        y = input.y;
    }

    private void LateUpdate()
    {
        transform.Rotate(transform.up, x * sensivityX);

        xRotation -= y * sensivityY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
        Vector3 targetRotation = cameraPoint.eulerAngles;
        targetRotation.x = xRotation;
        cameraPoint.eulerAngles = targetRotation;
    }
}
