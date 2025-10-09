using System.Collections.Generic;

[System.Serializable]
public abstract class QuestBase
{
    public int id;
    public string name;
    public string description;

    public QuestState state = QuestState.waitStart;

    public string ConvertToString()
    {
        string s = "{\n";
        s += "type: " + GetQuestTypeIndex() + "\n";
        s += "id: " + id + "\n";
        s += "name: " + name + "\n";
        s += "desc: " + description + "\n";
        s += "state: " + (int)state + "\n";
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
    }
    protected abstract int GetQuestTypeIndex();
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
}
