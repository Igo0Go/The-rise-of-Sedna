using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObject", menuName = "IgoGoTools/Items/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    public int id;
    public string itemName;
    public string actionDescription;
}
