using UnityEngine;

public class WeaponMagazine
{
    [Tooltip("���������� �������� � ��������"), Min(1)]
    public int maxAmmo = 1;
    [HideInInspector]
    public int currentAmmo;

    public WeaponMagazine(WeaponItemData data)
    {
        maxAmmo = currentAmmo = data.magazineSize;
    }
}
