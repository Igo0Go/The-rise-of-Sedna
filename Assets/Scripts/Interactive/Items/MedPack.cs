using UnityEngine;

public class MedPack : InteractiveObject
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private string ActionDescription;
    [SerializeField, Min(1)]
    private int hpPoints = 10;

    public override (string name, string action) GetData()
    {
        return (Name, ActionDescription + "\nВосстановить " + hpPoints + " ОЗ");
    }

    public override void Use()
    {
        if(FindFirstObjectByType<FPC_HealhSystem>().TryHeal(hpPoints))
        {
            Destroy(gameObject);
        }
    }
}
