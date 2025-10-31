using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public abstract class QuestBase
{
    public int id;
    public string name;
    public string description;
    public List<QuestDetails> details;

    public event Action<QuestBase> OnStateChanged;

    public QuestState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                OnStateChanged?.Invoke(this);
            }
        }
    }
    private QuestState _state = QuestState.waitStart;

    public virtual string ConvertToSaveString()
    {
        string s = "{\n";
        s += "type: " + GetQuestTypeIndex() + "\n";
        s += "id: " + id + "\n";
        s += "name: " + name + "\n";
        s += "desc: " + description + "\n";
        s += GetDetailsData() + "\n";
        s += "state: " + (int)_state + "\n";
        s += GetSpecificData() + "\n";
        s += "}\n";
        return s;
    }

    public QuestBase() { }
    public QuestBase(string[] settingsStrings)
    {
        id = int.Parse(settingsStrings[1]);
        name = settingsStrings[2];
        description = settingsStrings[3];
        details = SetDetailsData(settingsStrings[4]);
        _state = (QuestState)int.Parse(settingsStrings[5]);
        SetSpecificData(settingsStrings[6]);
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

public class ActivationQuest : QuestBase
{
    public List<int> activationObjectsIds;

    public ActivationQuest() { }

    public ActivationQuest(string[] settingsStrings) : base(settingsStrings)
    {
    }
    protected override int GetQuestTypeIndex()
    {
        return (int)QuestType.Activation;
    }
    protected override string GetSpecificData()
    {
        string s = "activationIds: [";
        foreach (int i in activationObjectsIds)
        {
            s += i + ",";
        }
        s += "]";
        return s;
    }

    protected override void SetSpecificData(string inputString)
    {
        string[] s = { "\n", "activationIds: ", "[", "]", ","};
        string[] dataStrings = inputString.Split(s, System.StringSplitOptions.RemoveEmptyEntries);

        activationObjectsIds = new List<int>();
        foreach (string i in dataStrings)
        {
            activationObjectsIds.Add(int.Parse(i));
        }
    }

    public void OnQuestTargetActivation(int  targetId)
    {
        if (activationObjectsIds.Contains(targetId))
        {
            activationObjectsIds.Remove(targetId);
        }

        if (activationObjectsIds.Count == 0)
        {
            State = QuestState.complete;
        }
    }
}
