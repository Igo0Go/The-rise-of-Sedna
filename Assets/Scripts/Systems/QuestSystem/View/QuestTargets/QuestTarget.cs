using UnityEngine;

public abstract class QuestTarget : MonoBehaviour
{
    [Min(0)]
    public int ID = 0;

    public abstract void UseQuestTarget();
}
