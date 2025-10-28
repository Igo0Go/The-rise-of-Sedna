using UnityEngine;

[CreateAssetMenu(fileName = "MagazineData", menuName = "IgoGoTools/Items/Magazines")]
public class MagazineData : InventoryObject
{
    public MagazineType type;
    [Tooltip("Количество патронов в магазине"), Min(1)]
    public int maxAmmo = 1;
}
