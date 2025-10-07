using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CutSceneSystem : MonoBehaviour
{
    private CutscenePack current;

    private ReplicSystem replicSystem;
    private PauseControlSystem pauseSystem;
    private FPC controls;
    private FPC.CutsceneActions cutsceneControls;

    private void Awake()
    {
        replicSystem = FindFirstObjectByType<ReplicSystem>();
        controls = new FPC();
        cutsceneControls = controls.Cutscene;
        cutsceneControls.Skip.performed += _ => OnSkip();
        pauseSystem = FindFirstObjectByType<PauseControlSystem>();
    }

    public void PlayCutScene(CutscenePack scene)
    {
        controls.Cutscene.Enable();
        pauseSystem.IsCutscene = true;

        current = scene;
        current.playable.Play();
        StartCoroutine(CheckCutSceneCoroutine(scene));
    }

    public void OnSkip()
    {
        if (current != null)
        {
            StopAllCoroutines();
            StartCoroutine(SkipCoroutine());
        }
    }

    private IEnumerator SkipCoroutine()
    {
        current.playable.time = current.playable.duration;
        yield return null;
        replicSystem.StopAndClear();
        current.onSkipEvent.Invoke();
        OnCutsceneEnded();
    }

    private IEnumerator CheckCutSceneCoroutine(CutscenePack pack)
    {
        float f = (float)pack.playable.duration;
        yield return new WaitForSeconds(f);
        OnCutsceneEnded();
    }

    private void OnCutsceneEnded()
    {
        controls.Cutscene.Disable();
        pauseSystem.IsCutscene = false;
        current = null;
    }
}

[System.Serializable]
public class CutscenePack
{
    public PlayableDirector playable;
    public UnityEvent onSkipEvent;
}
