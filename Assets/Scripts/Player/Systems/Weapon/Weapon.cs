using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform shootPoint;
    public WeaponItemData weaponData;

    public WeaponMagazine currentMagazine;
    private Transform cameraTransform;
    private float shootDelay;
    private bool shoot;
    private float currentTime;

    private Action shootAction;

    public void Init(Transform camera, WeaponMagazine magazine)
    {
        cameraTransform = camera;
        shootDelay = 1 / (weaponData.fireRate / 60);

        currentMagazine = magazine;

        switch (weaponData.shootMode)
        {
            case ShootMode.ClicToOneShot:
                shootAction = SpawnBullet;
                break;
            case ShootMode.PressToShoot:
                shootAction = StartShoot;
                break;
        }
    }

    public void AttackInput()
    {
        shootAction();
    }

    public void StopMainAttack()
    {
        shoot = false;
    }

    private void SpawnBullet()
    {
        if(currentMagazine.currentAmmo <= 0)
        {
            AudioPack.audioSystem.PlaySound(weaponData.noAmmoCLip);
            return;
        }

        currentMagazine.currentAmmo--;

        Bullet bullet = Instantiate(weaponData.bulletPrefab, shootPoint.position, shootPoint.rotation).
            GetComponent<Bullet>();

        Vector3 targetPoint;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo,
            weaponData.distance, ~weaponData.ignoreMask))
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = cameraTransform.position + cameraTransform.forward * weaponData.distance;
        }

        bullet.transform.forward = targetPoint - bullet.transform.position;
        bullet.LaunchBullet(weaponData.damage, weaponData.distance, weaponData.bulletSpeed, weaponData.ignoreMask);
        AudioPack.audioSystem.PlaySound(weaponData.shootCLip);
    }

    private void Update()
    {
        if(shoot)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                SpawnBullet();
                currentTime = shootDelay;
            }
        }
    }

    private void StartShoot()
    {
        shoot = true;
        currentTime = 0;
    }
}
