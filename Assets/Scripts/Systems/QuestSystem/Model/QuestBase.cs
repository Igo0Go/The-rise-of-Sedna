using System.Collections.Generic;

[System.Serializable]
public abstract class QuestBase
{
    public int id;
    public string name;
    public string description;

    public QuestState state = QuestState.waitStart;
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
}
