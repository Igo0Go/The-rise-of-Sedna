using UnityEngine;

public class EXP_Activator : InteractiveModule
{
    [SerializeField, Min(1)]
    private int EXP = 100;

    public override void Activate()
    {
        base.Activate();
        SkillHolder.Instance.AddExperience(EXP);
    }
}
