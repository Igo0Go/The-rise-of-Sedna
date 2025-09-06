using UnityEngine;

public class FPC_WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private Transform weaponPoint;
    [SerializeField]
    private Transform cameraTransform;

    private Weapon currentWeapon;

    public void MainAttack()
    {
        currentWeapon?.AttackInput();
    }
    public void StopMainAttack()
    {
        currentWeapon?.StopMainAttack();
    }
    public void TakeWeapon(GameObject weapon)
    {
        currentWeapon = Instantiate(weapon, weaponPoint).GetComponent<Weapon>();
        currentWeapon.Init(cameraTransform);
    }
}
