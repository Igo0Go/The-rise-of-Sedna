using UnityEngine;

[CreateAssetMenu(fileName = "ScreenMessagePack", menuName = "IgoGoTools/ScreenMessagePack")]
public class ScreenMessagePack : ScriptableObject
{
    public string messageName;

    [TextArea(3, 10)]
    public string messageText;
}
