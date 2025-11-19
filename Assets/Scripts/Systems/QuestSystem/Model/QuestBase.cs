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
    public List<QuestDetails> details;

    public event Action<QuestBase> OnStateChanged;

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
                    OnActivateQuest();
                }
                if (value == QuestState.complete && _state != QuestState.complete)
                {
                    OnCompleteQuest();
                }

                _state = value;
                OnStateChanged?.Invoke(this);
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
    }
    protected abstract int GetQuestTypeIndex();
    protected abstract string GetSpecificData();
    protected abstract void SetSpecificData(string inputString);

    protected string GetDetailsData()
    {
        string s = "details: [";
        foreach (QuestDetails item in details)
        {
            s += "(" + item.text + "|" + (item.unblock? "1" : "0" ) + ")=-=";
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
}

public enum QuestState
{
    waitStart = 0,
    active = 1,
    faled = -1,
    complete = 2
}


