using UnityEngine;

public class FPC_WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private Weapon currentWeapon;

    public void MainAttack()
    {
        currentWeapon?.AttackInput();
    }
    public void StopMainAttack()
    {
        currentWeapon?.StopMainAttack();
    }
}
