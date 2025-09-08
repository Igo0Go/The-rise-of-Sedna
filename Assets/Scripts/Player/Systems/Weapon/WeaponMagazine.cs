using UnityEngine;

public class WeaponMagazine
{
    [Tooltip("Количество патронов в магазине"), Min(1)]
    public int maxAmmo = 1;
    [HideInInspector]
    public int currentAmmo;

    public WeaponMagazine(WeaponItemData data)
    {
        maxAmmo = currentAmmo = data.magazineSize;
    }
}
