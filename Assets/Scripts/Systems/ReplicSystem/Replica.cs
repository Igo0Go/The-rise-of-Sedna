using UnityEngine;

[System.Serializable]
public class Replica
{
    [TextArea(3,7)]
    public string replicaText;
    public string speakerName;
    public AudioClip woiceClip;
}
