using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObject", menuName = "IgoGoTools/Items/InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public string actionDescription;
    public GameObject itemPrefab;
}
