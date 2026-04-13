using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public abstract class QuestBase
{
    public int id;
    public string name;
    public string description;
    public bool containceNewInfo = true;
    public List<QuestDetails> details;
    public int exp = 0;
    public List<int> completedQuestsToStart;

    public event Action<QuestBase> StateChanged;

    public QuestState State
    {
        get => _state;
        set
        {
            if(_state == QuestState.complete || _state == QuestState.faled)
            {
                return;
            }

            if (_state != value)
            {
                switch(value)
                {
                    case QuestState.active:
                        OnActivateQuest();
                        break;
                    case QuestState.complete:
                        OnCompleteQuest();
                        break;
                    default:
                        break;
                }
                _state = value;
                StateChanged?.Invoke(this);
            }
        }
    }
    private QuestState _state = QuestState.waitStart;

    public QuestBase() { }

    #region Сохранение
    public virtual string ConvertToSaveString()
    {
        string s = "{\r\n";
        s += "type: " + GetQuestTypeIndex() + "\n";
        s += "id: " + id + "\n";
        s += "name: " + name + "\n";
        s += "desc: " + description + "\n";
        s += "exp: " + exp + "\n";
        s += "newInfo: " + (containceNewInfo ? 1 : 0) + "\n";
        s += GetDetailsData() + "\n";
        s += "state: " + (int)_state + "\n";
        s += GetSpecificData() + "\n";
        s += GetCompletedQuestsData() + "\n";
        s += "}\r\n";
        return s;
    }
    protected abstract int GetQuestTypeIndex();
    protected string GetDetailsData()
    {
        string s = "details: [";
        foreach (QuestDetails item in details)
        {
            s += "(" + item.text + "|" + (item.unblock ? "1" : "0") + "|" + item.EXP + ")=-=";
        }
        s += "]";
        return s;
    }
    protected abstract string GetSpecificData();
    protected string GetCompletedQuestsData()
    {
        string s = "completedQuests: [";
        foreach (int i in completedQuestsToStart)
        {
            s += i + ",";
        }
        s += "]";
        return s;
    }
    #endregion

    #region Загрузка
    public QuestBase(string[] settingsStrings)
    {
        id = int.Parse(settingsStrings[1]);
        name = settingsStrings[2];
        description = settingsStrings[3];
        exp = int.Parse(settingsStrings[4]);
        containceNewInfo = settingsStrings[5] == "1";
        details = SetDetailsData(settingsStrings[6]);
        _state = (QuestState)int.Parse(settingsStrings[7]);
        SetSpecificData(settingsStrings[8]);
        SetCompletedQuestsData(settingsStrings[9]);
    }
    protected List<QuestDetails> SetDetailsData(string inputString)
    {
        List<QuestDetails> details = new List<QuestDetails>();

        string[] s = { "\n", "details: ", "[", "]", "=-=" };
        string[] dataStrings = inputString.Split(s, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string str in dataStrings)
        {
            string[] separators = { "(", ")", "|" };
            string[] parts = str.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            QuestDetails det = new QuestDetails();
            det.text = parts[0];
            det.unblock = parts[1] == "1";
            det.EXP = int.Parse(parts[2]);
            details.Add(det);
        }

        return details;
    }
    protected abstract void SetSpecificData(string inputString);
    protected void SetCompletedQuestsData(string inputString)
    {
        string[] s = { "\n", "completedQuests: ", "[", "]", "," };
        string[] dataStrings = inputString.Split(s, System.StringSplitOptions.RemoveEmptyEntries);

        completedQuestsToStart = new List<int>();
        foreach (string i in dataStrings)
        {
            completedQuestsToStart.Add(int.Parse(i));
        }
    }
    #endregion

    protected virtual void OnCompleteQuest()
    {
        SkillHolder.Instance.AddExperience(exp);
    }
    protected virtual void OnActivateQuest()
    {
        
    }

    public void UnblockDetailInfo(int index)
    {
        if (details[index].unblock)
        {
            return;
        }
        containceNewInfo = true;
        details[index].unblock = true;
        SkillHolder.Instance.AddExperience(details[index].EXP);
    }
}

[Serializable]
public class QuestDetails
{
    [TextArea(4, 8)]
    public string text;
    public bool unblock;
    public int EXP;
}

public enum QuestState
{
    waitStart = 0,
    active = 1,
    faled = -1,
    complete = 2
}