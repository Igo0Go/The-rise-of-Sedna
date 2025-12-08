using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public abstract class QuestBase
{
    public int id;
    public string name;
    public string description;
    public bool dirty = true;
    public int exp = 0;
    public List<int> completedQuestsToStart;
    public List<QuestDetails> details;

    public event Action<QuestBase> StateChanged;

    public QuestState State
    {
        get => _state;
        set
        {
            if(_state == QuestState.complete)
            {
                return;
            }


            if (_state != value)
            {
                if(value == QuestState.active && _state != QuestState.active)
                {
                    _state = value;
                    OnActivateQuest();
                }
                if (value == QuestState.complete && _state != QuestState.complete)
                {
                    _state = value;
                    OnCompleteQuest();
                }
                if(value == QuestState.faled && _state != QuestState.faled)
                {
                    _state = value;
                }
                StateChanged?.Invoke(this);
            }
        }
    }
    private QuestState _state = QuestState.waitStart;

    public virtual string ConvertToSaveString()
    {
        string s = "{\r\n";
        s += "type: " + GetQuestTypeIndex() + "\n";
        s += "id: " + id + "\n";
        s += "name: " + name + "\n";
        s += "desc: " + description + "\n";
        s += "exp: " + exp + "\n";
        s += "dirty: " + (dirty ? 1 : 0) + "\n";
        s += GetDetailsData() + "\n";
        s += "state: " + (int)_state + "\n";
        s += GetSpecificData() + "\n";
        s += GetCompletedQuestsData() + "\n";
        s += "}\r\n";
        return s;
    }

    protected virtual void OnCompleteQuest()
    {
        SkillHolder.Instance.AddExperience(exp);
    }
    protected virtual void OnActivateQuest()
    {
        
    }

    public QuestBase() { }
    public QuestBase(string[] settingsStrings)
    {
        id = int.Parse(settingsStrings[1]);
        name = settingsStrings[2];
        description = settingsStrings[3];
        exp = int.Parse(settingsStrings[4]);
        dirty = settingsStrings[5] == "1";
        details = SetDetailsData(settingsStrings[6]);
        _state = (QuestState)int.Parse(settingsStrings[7]);
        SetSpecificData(settingsStrings[8]);
        SetCompletedQuestsData(settingsStrings[9]);
    }
    protected abstract int GetQuestTypeIndex();
    protected abstract string GetSpecificData();
    protected abstract void SetSpecificData(string inputString);
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

    protected string GetDetailsData()
    {
        string s = "details: [";
        foreach (QuestDetails item in details)
        {
            s += "(" + item.text + "|" + (item.unblock? "1" : "0" ) + "|" + item.EXP + ")=-=";
        }
        s += "]";
        return s;
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


