using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FPC_WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private Transform weaponPoint;
    [SerializeField]
    private Transform cameraTransform;

    public Weapon currentWeapon;

    private Dictionary<MagazineType, List<WeaponMagazine>> magazines = new();

    public void MainAttack()
    {
        if (weaponPoint == null) return;
        currentWeapon.AttackInput();
    }
    public void StopMainAttack()
    {
        if (weaponPoint == null) return;
        currentWeapon.StopMainAttack();
    }
    public void TakeWeapon(WeaponItem weapon)
    {
        if(currentWeapon != null)
        {
            Instantiate(currentWeapon.weaponData.weaponItem, cameraTransform.position + cameraTransform.forward,
                Quaternion.identity).GetComponent<WeaponItem>().magazine = currentWeapon.currentMagazine;
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = Instantiate(weapon.weaponItemData.weaponPrefab, weaponPoint).GetComponent<Weapon>();
        currentWeapon.Init(cameraTransform, weapon.magazine);
    }
    public void AddMagazine(WeaponMagazine magazine)
    {
        if(!magazines.Keys.Contains(magazine.data.type))
        {
            magazines.Add(magazine.data.type, new List<WeaponMagazine>() { magazine });
        }
        else
        {
            magazines[magazine.data.type].Add(magazine);
        }
    }
    public void TryReload()
    {
        if(currentWeapon == null) return;

        if (currentWeapon.reload) return;

        if(magazines.Keys.Contains(currentWeapon.weaponData.MagazineType))
        {
            if (magazines[currentWeapon.weaponData.MagazineType].Count > 0)
            {
                WeaponMagazine m = magazines[currentWeapon.weaponData.MagazineType][0];
                magazines[currentWeapon.weaponData.MagazineType].RemoveAt(0);
                currentWeapon.Reload(m);
            }
        }
    }
}
