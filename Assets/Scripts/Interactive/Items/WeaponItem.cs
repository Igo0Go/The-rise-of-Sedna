using UnityEngine;

public class WeaponItem : InteractiveObject
{
    [SerializeField]
    private WeaponItemData weaponItemData;

    public override (string name, string action) GetData()
    {
        return (weaponItemData.Name, weaponItemData.ActionDescription);
    }

    public override void Use()
    {
        FindFirstObjectByType<FPC_WeaponSystem>().TakeWeapon(weaponItemData.weaponPrefab);
        Destroy(gameObject);
    }
}
