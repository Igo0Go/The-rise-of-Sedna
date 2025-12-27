using System;

public class CollectingQuestTargetModule : QuestTarget
{
    public event Action<int> TryCompleteSearchQuest;

    public override void UseQuestTarget()
    {
        TryCompleteSearchQuest.Invoke(ID);
    }
}
