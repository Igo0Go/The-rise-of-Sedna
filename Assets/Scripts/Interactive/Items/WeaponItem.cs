using UnityEngine;

public class WeaponItem : InteractiveObject
{
    public WeaponItemData weaponItemData;
    public WeaponMagazine magazine;

    [SerializeField]
    private GameObject magazineObject;

    public void SetMagazine(WeaponMagazine m)
    {
        magazine = m;
        magazineObject.SetActive(magazine != null);
    }

    public override (string name, string action) GetData()
    {
        string title = weaponItemData.Name;

        if (magazine != null)
        {
            int maxShot = magazine.data.maxAmmo / weaponItemData.consumptionPerShot;
            int currentShotCount = magazine.currentAmmo / weaponItemData.consumptionPerShot;
            title += "(" + currentShotCount + "/" + maxShot + ")";
        }
        else
        {
            title += " (разряжено)";
        }

        return (title, weaponItemData.ActionDescription);
    }

    public override void Use()
    {
        FindFirstObjectByType<FPC_WeaponSystem>().TakeWeapon(this);
        Destroy(gameObject);
    }
}
