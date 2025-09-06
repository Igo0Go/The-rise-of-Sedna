using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public abstract (string name, string action) GetData();
    public abstract void Use();
}
