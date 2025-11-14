using UnityEngine;
using System;
using System.Collections.Generic;

public class ManualInteractive : InteractiveObject
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private ManualActionType actionType = ManualActionType.Toggle;
    [SerializeField]
    private string FirstUseDescription;
    [SerializeField]
    private string SecondUseDescription;
    [SerializeField]
    private bool npc_use_targetState = false;

    private bool isActive = false;

    [SerializeField]
    private List<InteractiveModule> modules;

    private void Awake()
    {
        switch (actionType)
        {
            case ManualActionType.ActivateOnly:
                useAction = ActivateAction;
                break;
           case ManualActionType.DeactivateOnly:
                useAction = DeactivateAction;
                break;
            case ManualActionType.Toggle:
                useAction = ToggleAction;
                break;
        }
    }

    public override (string name, string action) GetData()
    {
        return (itemName, isActive? SecondUseDescription : FirstUseDescription);
    }

    public override void Use()
    {
        useAction();
        onUseEvent?.Invoke();
    }

    private Action useAction;

    private void ToggleAction()
    {
        foreach (var module in modules)
        {
            module.Switch();
            isActive = !isActive;
        }
    }
    private void ActivateAction()
    {
        foreach(var module in modules)
        {
            module.Activate();
        }
    }
    private void DeactivateAction()
    {
        foreach( var module in modules)
        {
            module.Deactivate();
        }
    }

    public void NPC_Use()
    {
        if(npc_use_targetState)
        {
            ActivateAction();
        }
        else
        {
            DeactivateAction();
        }
    }
}

public enum ManualActionType
{
    ActivateOnly,
    DeactivateOnly,
    Toggle
}
