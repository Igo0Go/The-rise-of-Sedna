using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class InteractiveArea : Actor
{
    [SerializeField]
    private ActionType enterActionType;
    [SerializeField]
    private ActionType exitActionType;
    [SerializeField]
    private AreaPostActionMode postActionModeAfterEnter;
    [SerializeField]
    private AreaPostActionMode postActionModeAfterExit;

    private Action enterAction, exitAction, postEnterAction, postExitAction;

    private void Awake()
    {
        switch(enterActionType)
        {
            case ActionType.activateModules:
                enterAction = base.Activate;
                break;
            case ActionType.deactivateModules:
                enterAction = base.Deactivate;
                break;
            case ActionType.switchActiveModules:
                enterAction = base.Switch;
                break;
            case ActionType.modulesToDefault:
                enterAction = base.ToDefaultState;
                break;
            case ActionType.empty:
                enterAction = ()=> { };
                break;
        }

        switch (exitActionType)
        {
            case ActionType.activateModules:
                exitAction = base.Activate;
                break;
            case ActionType.deactivateModules:
                exitAction = base.Deactivate;
                break;
            case ActionType.switchActiveModules:
                exitAction = base.Switch;
                break;
            case ActionType.modulesToDefault:
                exitAction = base.ToDefaultState;
                break;
            case ActionType.empty:
                exitAction = () => { };
                break;
        }

        switch(postActionModeAfterEnter)
        {
            case AreaPostActionMode.empty:
                postEnterAction = () => { };
                break;
            case AreaPostActionMode.delete:
                postEnterAction = () => { Destroy(gameObject); };
                break;
        }

        switch (postActionModeAfterExit)
        {
            case AreaPostActionMode.empty:
                postExitAction = () => { };
                break;
            case AreaPostActionMode.delete:
                postExitAction = () => { Destroy(gameObject); };
                break;
        }
    }

    public void OnEnter()
    {
        enterAction();
        postEnterAction();
    }
    public void OnExit()
    {
        exitAction();
        postExitAction();
    }
}

public enum AreaPostActionMode
{
    delete,
    empty
}
public enum ActionType
{
    activateModules,
    deactivateModules,
    switchActiveModules,
    modulesToDefault,
    empty
}
