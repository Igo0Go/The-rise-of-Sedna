using UnityEngine;
using UnityEngine.Events;

public class ReplicModule : InteractiveModule
{
    [SerializeField]
    private ReplicModulePack pack;

    public override void Activate()
    {
        FindFirstObjectByType<ReplicSystem>().AddReplicaPackToQueue(pack);
    }
}
[System.Serializable]
public class ReplicModulePack
{
    public ReplicPack replicPack;
    public UnityEvent postReplicAction;
}