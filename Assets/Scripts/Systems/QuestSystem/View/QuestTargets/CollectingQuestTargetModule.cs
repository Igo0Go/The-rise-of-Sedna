using System;

public class CollectingQuestTargetModule : QuestTarget
{
    public event Action<int> TryCompleteCollectingQuest;

    public override void UseQuestTarget()
    {
        TryCompleteCollectingQuest.Invoke(ID);
    }
}
