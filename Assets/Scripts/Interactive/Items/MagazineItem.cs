using UnityEngine;

public class MagazineItem : InteractiveObject
{
    [SerializeField]
    private WeaponMagazine magazine;

    public override (string name, string action) GetData()
    {
        return (magazine.data.itemName + " (" + magazine.currentAmmo + "/" + magazine.data.maxAmmo + ")",
            magazine.data.actionDescription);
    }

    public override void Use()
    {
        InventarySystem.Instance.AddConcreteMagazine(magazine);
        onUseEvent?.Invoke();
        Destroy(gameObject);
    }
}
