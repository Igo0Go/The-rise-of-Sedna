using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationModule : InteractiveModule
{
    [SerializeField]
    private string animBoolParameterName;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
