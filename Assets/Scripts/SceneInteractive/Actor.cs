using UnityEngine;
using System.Collections.Generic;

public class Actor : InteractiveModule
{
    [SerializeField]
    protected List<InteractiveModule> modules;

    [Min(1)]
    public int NpcWorkPoints;
    [SerializeField]
    private bool NpcTargetState = true;

    public override void Activate()
    {
        base.Activate();
        foreach (var module in modules)
        {
            module.Activate();
        }
    }
    public override void Deactivate()
    {
        base.Deactivate();
        foreach (var module in modules)
        {
            module.Deactivate();
        }
    }
    public override void ToDefaultState()
    {
        base.ToDefaultState();
        foreach (var module in modules)
        {
            module.ToDefaultState();
        }
    }

    public void NPCAction()
    {
        if (NpcTargetState)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

}
