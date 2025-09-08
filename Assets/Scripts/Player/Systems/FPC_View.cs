using System;
using System.Collections;
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
    [SerializeField, Range(0,1)]
    private float recoilForce = 1;
    [SerializeField, Range(0, 1)]
    private float recoilReturnForce = 1;
    [SerializeField, Min(1)]
    private float recoilSpeed = 1;
    [SerializeField]
    private Transform recoilPoint;

    private float x, y, xRecoil, yRecoil;
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
        transform.Rotate(transform.up, x * sensivityX + xRecoil);

        xRotation -= y * sensivityY + yRecoil;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
        Vector3 targetRotation = cameraPoint.eulerAngles;
        targetRotation.x = xRotation;
        cameraPoint.eulerAngles = targetRotation;
    }

    private Coroutine currentRecoilCoroutine;
    public void OnRecoil(Vector2 input)
    {
        if (currentRecoilCoroutine != null)
        {
            StopCoroutine(currentRecoilCoroutine);
        }
        currentRecoilCoroutine = StartCoroutine(SmoothRecoil(input));
    }

    private IEnumerator SmoothRecoil(Vector2 input)
    {
        xRecoil = yRecoil = 0;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * recoilSpeed;

            xRecoil = Mathf.Lerp(0, input.x * recoilForce, t);
            yRecoil = Mathf.Lerp(0, input.y * recoilForce, t);
            yield return null;
        }

        t = 0;

        while (t < 1 * recoilReturnForce)
        {
            t += Time.deltaTime * recoilSpeed;

            xRecoil = Mathf.Lerp(0, -input.x * recoilForce, t);
            yRecoil = Mathf.Lerp(0, -input.y * recoilForce, t);
            yield return null;
        }

        xRecoil = yRecoil = 0;

        //t = 0;

        //while (t < 1*recoilReturnForce)
        //{
        //    t += Time.deltaTime * recoilSpeed;
        //    transform.localEulerAngles = Vector3.Lerp(targetY, saveY, t);
        //    cameraPoint.localEulerAngles = Vector3.Lerp(targetX, saveX, t);
        //    yield return null;
        //}
    }
}
