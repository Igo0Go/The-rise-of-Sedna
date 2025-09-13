using UnityEngine;

public abstract class InteractiveModule : MonoBehaviour
{
    public bool IsActive { get; protected set; } = false;

    public virtual void Activate() => IsActive = true;
    public virtual void Deactivate() => IsActive = false;
    public virtual void ToDefaultState() => IsActive = false;
    public virtual void Switch()
    {
        IsActive = !IsActive;
        if (IsActive)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }
}
