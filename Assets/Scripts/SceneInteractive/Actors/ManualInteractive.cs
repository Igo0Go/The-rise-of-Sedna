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
    [SerializeField]
    private bool opportunityToDestroy = false;
    [SerializeField]
    private bool onDestroyTargetState = false;

    private bool isActive = false;
    private bool destroyed = false;
    private bool blocked = false;


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
        if(blocked)
        {
            return (itemName, string.Empty);
        }

        return (itemName, isActive? SecondUseDescription : FirstUseDescription);
    }

    public override void Use()
    {
        if(blocked) return;

        useAction();
        onUseEvent?.Invoke();
    }

    private Action useAction;

    private void ToggleAction()
    {
        if(destroyed) return;

        foreach (var module in modules)
        {
            module.Switch();
            isActive = !isActive;
        }
    }
    private void ActivateAction()
    {
        if (destroyed) return;
        foreach (var module in modules)
        {
            module.Activate();
        }
    }
    private void DeactivateAction()
    {
        if (destroyed) return;
        foreach ( var module in modules)
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
    public void OnDestroyFromWeapon()
    {
        if(!opportunityToDestroy)
        {
            return;
        }

        if(destroyed) return;
        if(onDestroyTargetState)
        {
            onUseEvent?.Invoke();
            ActivateAction();
        }
        else
        {
            DeactivateAction();
        }
        destroyed = true;
    }
    public void SetBlock(bool value)
    {
        blocked = value;
    }
}

public enum ManualActionType
{
    ActivateOnly,
    DeactivateOnly,
    Toggle
}
