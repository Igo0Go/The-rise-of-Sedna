using System.Collections.Generic;

[System.Serializable]
public abstract class QuestBase
{
    public int id;
    public string name;
    public string description;

    public QuestState state = QuestState.waitStart;

    public virtual string ConvertToSaveString()
    {
        string s = "{\n";
        s += "type: " + GetQuestTypeIndex() + "\n";
        s += "id: " + id + "\n";
        s += "name: " + name + "\n";
        s += "desc: " + description + "\n";
        s += "state: " + (int)state + "\n";
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
        state = (QuestState)int.Parse(settingsStrings[4]);
        SetSpecificData(settingsStrings[5]);
    }
    protected abstract int GetQuestTypeIndex();
    protected abstract string GetSpecificData();
    protected abstract void SetSpecificData(string inputString);
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
}
