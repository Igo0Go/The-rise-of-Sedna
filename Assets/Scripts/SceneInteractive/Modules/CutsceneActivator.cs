using UnityEngine;

public class CutsceneActivator : InteractiveModule
{
    [SerializeField]
    private CutscenePack pack;

    public override void Activate()
    {
        FindFirstObjectByType<CutSceneSystem>().PlayCutScene(pack);
        base.Activate();
    }
}
