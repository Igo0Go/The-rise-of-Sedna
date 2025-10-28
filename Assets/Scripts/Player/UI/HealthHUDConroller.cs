using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthHudConroller : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Image damagePanel;
    [SerializeField]
    private TMP_Text medPackCountText;

    private void Awake()
    {
        FPC_HealhSystem healthSystem = FindFirstObjectByType<FPC_HealhSystem>();
        healthSystem.OnHealthStateChanged += OnHealthChanged;
        healthSystem.OnDamage += OnDamage;
        healthSystem.MedPackCountChanged += OnMedPackCountChanged;
        SetValueForDamagePanel(0);
    }

    private void OnHealthChanged(int health, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    private void OnDamage()
    {
        StopAllCoroutines();
        SetValueForDamagePanel(1);
        StartCoroutine(ChangeDamageValue());
    }

    private void SetValueForDamagePanel(float value)
    {
        damagePanel.color = new Color(damagePanel.color.r, damagePanel.color.g, damagePanel.color.b, value);
    }

    private IEnumerator ChangeDamageValue()
    {
        float t = 1;
        while (t > 0)
        {
            SetValueForDamagePanel(t);
            yield return null;
            t -= Time.deltaTime;
        }
        SetValueForDamagePanel(0);
    }

    private void OnMedPackCountChanged(int medPackCount)
    {
        medPackCountText.text = medPackCount.ToString();
    }
}
