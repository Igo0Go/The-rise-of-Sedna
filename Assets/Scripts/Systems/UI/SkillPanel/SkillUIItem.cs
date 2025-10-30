using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private Button levelUpButton;

    private SkillInfo saved;

    public void Init(SkillInfo skillInfo)
    {
        saved = skillInfo;
        skillInfo.OnLevelUp += UpdateInfo;
        levelUpButton.onClick.AddListener(OnClick);
    }

    public void UpdateInfo()
    {
        nameText.text = saved.name;
        descriptionText.text = saved.description;
        levelUpButton.gameObject.SetActive(saved.CanLevelUp && 
            SkillHolder.Instance.SkillPoints > 0);
    }

    public void OnClick()
    {
        SkillHolder.Instance.LevelUpSkill(saved);
    }
}
