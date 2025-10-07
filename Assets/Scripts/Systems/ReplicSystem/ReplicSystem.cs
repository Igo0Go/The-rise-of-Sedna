using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReplicSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject subtitlesPanel;
    [SerializeField]
    private AudioSource woiceSource;
    [SerializeField]
    private TMP_Text speakerNameText;
    [SerializeField]
    private TMP_Text replicaSubtitlesText;

    private List<ReplicModulePack> currentQueue = new List<ReplicModulePack>();
    private bool replicsInWorks;

    private void Awake()
    {
        woiceSource.loop = false;
        subtitlesPanel.SetActive(false);
    }

    public void AddReplicaPackToQueue(ReplicModulePack replicPack)
    {
        currentQueue.Add(replicPack);
        StartPlayReplics();
    }

    public void PlayReplicsImmidiatly(ReplicModulePack pack)
    {
        currentQueue.Clear();
        currentQueue.Add(pack);
        StartPlayReplics();
    }

    public void StopAndClear()
    {
        StopAllCoroutines();
        currentQueue.Clear();
        woiceSource.Stop();
        subtitlesPanel.SetActive(false);
        replicsInWorks = false;
    }

    private void StartPlayReplics()
    {
        if (!replicsInWorks)
        {
            replicsInWorks = true;
            StartCoroutine(PlayReplicsCoroutine());
        }
    }

    private IEnumerator PlayReplicsCoroutine()
    {
        subtitlesPanel.SetActive(true);

        while (currentQueue.Count > 0)
        {
            ReplicPack pack = currentQueue[0].replicPack;

            foreach (Replica replica in pack.replics)
            {
                speakerNameText.text = replica.speakerName;
                replicaSubtitlesText.text = replica.replicaText;

                woiceSource.PlayOneShot(replica.woiceClip);

                while(woiceSource.isPlaying)
                {
                    yield return new WaitForSeconds(1);
                }
            }

            currentQueue[0].postReplicAction.Invoke();
            currentQueue.RemoveAt(0);
        }

        subtitlesPanel.SetActive(false);
        replicsInWorks = false;
    }
}


