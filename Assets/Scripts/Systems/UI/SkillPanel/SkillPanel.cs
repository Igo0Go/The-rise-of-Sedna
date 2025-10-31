using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject skillItemPrefab;
    [SerializeField]
    private Transform skillItemsContainer;
    [SerializeField]
    private GameObject panelObject;
    [SerializeField]
    private TMP_Text skillPointsText;
    [SerializeField]
    private TMP_Text EXPText;

    private List<SkillUIItem> skillUIItems = new List<SkillUIItem>();
    private PauseControlSystem pauseSystem;

    private void Start()
    {
        pauseSystem = FindFirstObjectByType<PauseControlSystem>();
        SpawnAllSkillItems();
        SkillHolder.Instance.AddNewExp += OnNewExp;
        SkillHolder.Instance.SkillPointsChanged += OnNewSkillPoint;
    }

    public void PanelToggle()
    {
        if(panelObject.activeSelf)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        pauseSystem.IsSkillPanel = true;
        panelObject.SetActive(true);
        UpdateAll();
    }
    public void HidePanel()
    {
        pauseSystem.IsSkillPanel = false;
        panelObject.SetActive(false);
    }

    private void SpawnAllSkillItems()
    {
        SkillUIItem skillUIItem = Instantiate(skillItemPrefab, skillItemsContainer).GetComponent<SkillUIItem>();
        skillUIItem. Init(SkillHolder.Instance.speed);
        skillUIItems.Add(skillUIItem);

        skillUIItem = Instantiate(skillItemPrefab, skillItemsContainer).GetComponent<SkillUIItem>();
        skillUIItem.Init(SkillHolder.Instance.sprintMultiplier);
        skillUIItems.Add(skillUIItem);

        skillUIItem = Instantiate(skillItemPrefab, skillItemsContainer).GetComponent<SkillUIItem>();
        skillUIItem.Init(SkillHolder.Instance.sprintTime);
        skillUIItems.Add(skillUIItem);

        skillUIItem = Instantiate(skillItemPrefab, skillItemsContainer).GetComponent<SkillUIItem>();
        skillUIItem.Init(SkillHolder.Instance.jumpHeight);
        skillUIItems.Add(skillUIItem);

        skillUIItem = Instantiate(skillItemPrefab, skillItemsContainer).GetComponent<SkillUIItem>();
        skillUIItem.Init(SkillHolder.Instance.inAirMoveForce);
        skillUIItems.Add(skillUIItem);

        skillUIItem = Instantiate(skillItemPrefab, skillItemsContainer).GetComponent<SkillUIItem>();
        skillUIItem.Init(SkillHolder.Instance.recoilForceMultiplier);
        skillUIItems.Add(skillUIItem);

        UpdateAll();
    }

    private void UpdateAll()
    {
        skillPointsText.text = SkillHolder.Instance.SkillPoints.ToString();
        EXPText.text = SkillHolder.Instance.CurrentExp + "/" + SkillHolder.Instance.CurrentExpTargetLevel;

        foreach (var item in skillUIItems)
        {
            item.UpdateInfo();
        }
    }

    public void OnNewExp(int  exp)
    {
       LogPanel.instance.ShowStringInLog("Очки опыта: +" + exp);
    }
    public void OnNewSkillPoint(int skillPoint)
    {
        UpdateAll();
        LogPanel.instance.ShowStringInLog("Очков навыков: " + skillPoint);
    }
}
