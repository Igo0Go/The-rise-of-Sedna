using UnityEngine;

[CreateAssetMenu(fileName = "ObjectivePack", menuName = "IgoGoTools/ObjectivePack")]
public class ObjectivePack : ScriptableObject
{
    [TextArea(3,10)]
    public string taskString;
}
