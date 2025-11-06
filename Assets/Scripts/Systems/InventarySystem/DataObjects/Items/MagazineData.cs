using UnityEngine;

[CreateAssetMenu(fileName = "MagazineData", menuName = "IgoGoTools/Items/Magazines")]
public class MagazineData : InventoryItemData
{
    public MagazineType type;
    [Tooltip(" оличество патронов в магазине"), Min(1)]
    public int maxAmmo = 1;
    [Tooltip("ћаксимальное количество переносимых магазинов"), Min(1)]
    public int maxMagazine = 1;

    public WeaponMagazine GetMagazine()
    {
        return new WeaponMagazine()
        {
            data = this,
            currentAmmo = maxAmmo
        };
    }
}
