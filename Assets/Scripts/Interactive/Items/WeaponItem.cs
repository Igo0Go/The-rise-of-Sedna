public class WeaponItem : InteractiveObject
{
    public WeaponItemData weaponItemData;
    public WeaponMagazine magazine;


    public override (string name, string action) GetData()
    {
        int maxShot = magazine.data.maxAmmo / weaponItemData.consumptionPerShot;
        int currentShotCount = magazine.currentAmmo / weaponItemData.consumptionPerShot;

        return (weaponItemData.Name + "(" + currentShotCount +"/" + maxShot +")", 
            weaponItemData.ActionDescription);
    }

    public override void Use()
    {
        FindFirstObjectByType<FPC_WeaponSystem>().TakeWeapon(this);
        Destroy(gameObject);
    }
}
