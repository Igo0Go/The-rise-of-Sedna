using System.Collections.Generic;

public class Quest_Activation : QuestBase
{
    public List<int> activationObjectsIds;

    public Quest_Activation() { }

    public Quest_Activation(string[] settingsStrings) : base(settingsStrings)
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
        string[] s = { "\n", "activationIds: ", "[", "]", "," };
        string[] dataStrings = inputString.Split(s, System.StringSplitOptions.RemoveEmptyEntries);

        activationObjectsIds = new List<int>();
        foreach (string i in dataStrings)
        {
            activationObjectsIds.Add(int.Parse(i));
        }
    }

    public void OnQuestTargetActivation(int targetId)
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
