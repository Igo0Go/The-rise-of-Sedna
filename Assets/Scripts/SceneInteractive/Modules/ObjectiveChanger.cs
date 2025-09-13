using System;
using UnityEngine;

public class ObjectiveChanger : InteractiveModule
{
    [SerializeField]
    private ObjectivePack pack;

    private event Action<ObjectivePack> ObjectiveChanged;

    private void Awake()
    {
        ObjectiveChanged += FindFirstObjectByType<ObjectiveSystem>().ShowNewObjective;
    }

    public override void Activate()
    {
        base.Activate();
        ObjectiveChanged?.Invoke(pack);
    }
}
