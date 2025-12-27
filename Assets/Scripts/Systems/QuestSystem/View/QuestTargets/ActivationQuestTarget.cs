using System;

public class ActivationQuestTarget : QuestTarget
{
    public event Action<ActivationQuestTarget> targetActivated;

    public override void UseQuestTarget()
    {
        targetActivated?.Invoke(this);
    }
}
