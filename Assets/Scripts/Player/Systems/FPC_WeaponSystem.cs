using UnityEngine;

public class FPC_WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private Transform weaponPoint;
    [SerializeField]
    private Transform cameraTransform;

    public Weapon currentWeapon;

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
}
