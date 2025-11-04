using UnityEngine;

public class FPC_HeadbobSystem : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField, Min(0)]
    private float frequency = 15;
    [SerializeField, Min(0)]
    private float amount = 1f;

    private Vector3 startPos;
    private Vector2 currentInput;

    private float frequencyMultiplier = 1;
    private float amountMultiplier = 1;

    private float aimMultiplier = 1;

    void Awake()
    {
        startPos = cameraTransform.localPosition;
    }

    void Update()
    {
        CheckForHeadbob();
        StopHeadBob();
    }

    public void SetInput(Vector2 value)
    {
        currentInput = value;
    }
    public void SetFrequency(float value)
    {
        frequencyMultiplier = value;
    }
    public void SetAmount(float value)
    {
        amountMultiplier = value;
    }

    public void SetAim(bool value)
    {
        aimMultiplier = value ? 0.2f : 1;
    }

    private void CheckForHeadbob()
    {
        if(currentInput.sqrMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private void StartHeadBob()
    {
        Vector3 pos = Vector3.zero;

        pos.y += Mathf.Sin(Time.time * frequency * frequencyMultiplier) * Time.deltaTime * amount * amountMultiplier;
        pos.x = Mathf.Cos(Time.time * frequency * frequencyMultiplier / 2) * Time.deltaTime * amount * amountMultiplier;

        cameraTransform.localPosition += pos * aimMultiplier;
    }

    private void StopHeadBob()
    {
        if(cameraTransform.localPosition == startPos)
        {
            return;
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, startPos, Time.deltaTime);
    }
}
