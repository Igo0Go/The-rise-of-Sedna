using UnityEngine;
using UnityEngine.UI;

public class StaminaSliderController : MonoBehaviour
{
    [SerializeField]
    private Slider staminaSlider;

    private void Awake()
    {
        FindAnyObjectByType<FPC_Movement>().SprintStatusChanged += OnStaminaStatusChanged;
        OnStaminaStatusChanged(0, 1, false);
    }

    private void OnStaminaStatusChanged(float currentValue, float maxValue, bool visible)
    {
        staminaSlider.gameObject.SetActive(visible);
        if (visible )
        {
            staminaSlider.maxValue = maxValue;
            staminaSlider.value = currentValue;
        }
    }
}
