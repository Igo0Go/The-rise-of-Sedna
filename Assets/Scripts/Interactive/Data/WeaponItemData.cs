using UnityEngine;

[CreateAssetMenu(menuName ="IgoGoTools/Items/Weapon")]
public class WeaponItemData : ScriptableObject
{
    public string Name;
    public string ActionDescription;
    public GameObject weaponPrefab;
}
