using UnityEngine;

public class AnimationModule : InteractiveModule
{
    [SerializeField]
    private string animBoolParameterName;
    [SerializeField]
    private Animator animator;

    public override void Activate()
    {
        base.Activate();
        animator.SetBool(animBoolParameterName, true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        animator.SetBool(animBoolParameterName, false);
    }
}
