using UnityEngine;

public class WeaponItem : InteractiveObject
{
    public WeaponItemData weaponItemData;
    public WeaponMagazine magazine;

    private void Awake()
    {
        magazine = new WeaponMagazine(weaponItemData);
    }

    public override (string name, string action) GetData()
    {
        return (weaponItemData.Name + "(" + magazine.currentAmmo +"/" + magazine.maxAmmo +")", 
            weaponItemData.ActionDescription);
    }

    public override void Use()
    {
        FindFirstObjectByType<FPC_WeaponSystem>().TakeWeapon(this);
        Destroy(gameObject);
    }
}
