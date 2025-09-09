using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform shootPoint;
    public WeaponItemData weaponData;

    [SerializeField]
    private GameObject magazineObj;

    public WeaponMagazine currentMagazine;
    private Transform cameraTransform;
    private float shootDelay;
    private bool shoot;
    private float currentTime;

    [HideInInspector]
    public bool reload;

    public event Action<Vector2> Recoil;
    public event Action AmmoChanged;
    public event Action ReloadFinaled;


    private Action shootAction;

    public void Init(Transform camera, WeaponMagazine magazine)
    {
        cameraTransform = camera;
        shootDelay = 1 / (weaponData.fireRate / 60);

        currentMagazine = magazine;

        if(currentMagazine == null)
        {
            magazineObj.SetActive(false);
        }

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
        if (reload) return;

        shootAction();
    }

    public void StopMainAttack()
    {
        shoot = false;
    }
        
    private void SpawnBullet()
    {
        if(currentMagazine == null || currentMagazine.currentAmmo < weaponData.consumptionPerShot)
        {
            AudioPack.audioSystem.PlaySound(weaponData.noAmmoCLip);
            return;
        }

        currentMagazine.currentAmmo -= weaponData.consumptionPerShot;
        AmmoChanged?.Invoke();

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
        Recoil?.Invoke(weaponData.recoilVector);
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

    public void Reload(WeaponMagazine magazine)
    {
        StartCoroutine(ReloadCoroutine(magazine));
    }

    public void PullOutMagazine()
    {
        if(magazineObj.activeSelf)
        {
            magazineObj.SetActive(false);
            AudioPack.audioSystem.PlaySound(weaponData.reloadCLip);
            AmmoChanged?.Invoke();
        }
    }
    public void InsertMagazine(WeaponMagazine m)
    {
        magazineObj.SetActive(true);
        AudioPack.audioSystem.PlaySound(weaponData.reloadCLip);
        currentMagazine = m;
        AmmoChanged?.Invoke();
    }

    public IEnumerator ReloadCoroutine(WeaponMagazine m)
    {
        reload = true;
        PullOutMagazine();
        yield return new WaitForSeconds(1);
        InsertMagazine(m);
        reload = false;
        ReloadFinaled?.Invoke();
    }
}
