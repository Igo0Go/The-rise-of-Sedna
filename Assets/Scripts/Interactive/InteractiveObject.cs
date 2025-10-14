using UnityEngine;
using UnityEngine.Events;

public abstract class InteractiveObject : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent onUseEvent;
    public abstract (string name, string action) GetData();
    public abstract void Use();
}
