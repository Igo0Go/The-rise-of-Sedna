using UnityEngine;

public class MagazineItem : InteractiveObject
{
    [SerializeField]
    private WeaponMagazine magazine;


    public override (string name, string action) GetData()
    {
        return (magazine.data.magazineName + " (" + magazine.currentAmmo + "/" + magazine.data.maxAmmo + ")",
            magazine.data.actionDescription);
    }

    public override void Use()
    {
        FindFirstObjectByType<FPC_WeaponSystem>().AddMagazine(magazine);
        Destroy(gameObject);
    }
}
