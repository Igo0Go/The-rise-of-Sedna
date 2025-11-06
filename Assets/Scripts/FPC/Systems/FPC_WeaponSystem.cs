using System;
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



    private event Action<Vector2> RecoilEvent;
    public event Action<Weapon> WeaponChanged;
    public event Action<Weapon> AmmoChanged;
    public event Action<bool> AimValueChanged;
    public event Action<List<WeaponMagazine>, Weapon> MagazinesForCurrentWeaponChanged;

    private float reloadTime = 0;

    private void Awake()
    {
        RecoilEvent += FindFirstObjectByType<FPC_View>().OnRecoil;
        AimValueChanged += FindFirstObjectByType<FPC_HeadbobSystem>().SetAim;
        AimValueChanged += FindFirstObjectByType<FPC_View>().SetAimState;
        InventarySystem.Instance.MagazinesChanged += OnInventaryMagazinesChanged;
        reloadTime = -1;
    }
    private void OnDestroy()
    {
        InventarySystem.Instance.MagazinesChanged -= OnCurrentWeaponAmmoChanged;
    }

    public void MainAttack()
    {
        if (currentWeapon == null) return;
        currentWeapon.AttackInput();
    }
    public void StopMainAttack()
    {
        if (currentWeapon == null) return;
        currentWeapon.StopMainAttack();
    }
    public void StartReload()
    {
        reloadTime = 0;
    }
    public void StopReload()
    {
        if (reloadTime > 1 || reloadTime < 0) return;
        if (currentWeapon == null) return;
        if (currentWeapon.reload) return;

        TryReload();
    }
    public void TakeWeapon(WeaponItem weapon)
    {
        if(currentWeapon != null)
        {
            currentWeapon.Recoil -= OnRecoil;
            currentWeapon.AmmoChanged -= OnCurrentWeaponAmmoChanged;
            currentWeapon.ReloadFinaled -= OnFinalReload;

            WeaponItem item = Instantiate(currentWeapon.weaponData.itemPrefab, cameraTransform.position + cameraTransform.forward,
                Quaternion.identity).GetComponent<WeaponItem>();

            item.SetMagazine(currentWeapon.currentMagazine);
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = Instantiate(weapon.weaponItemData.weaponPrefab, weaponPoint).GetComponent<Weapon>();
        currentWeapon.Init(cameraTransform, weapon.magazine);

        currentWeapon.Recoil += OnRecoil;
        currentWeapon.AmmoChanged += OnCurrentWeaponAmmoChanged;
        currentWeapon.ReloadFinaled += OnFinalReload;

        WeaponChanged?.Invoke(currentWeapon);
        AmmoChanged?.Invoke(currentWeapon);
        OnInventaryMagazinesChanged();
    }

    public void TryReload()
    {
        if(currentWeapon == null) return;
        if (currentWeapon.reload) return;

        List<WeaponMagazine> magazines =
            InventarySystem.Instance.GetMagazinesOfType(currentWeapon.weaponData.MagazineType);

        if (magazines.Count > 0)
        {
            if (currentWeapon.currentMagazine != null && currentWeapon.currentMagazine.currentAmmo != 0)
            {
                InventarySystem.Instance.AddConcreteMagazine(currentWeapon.currentMagazine);
            }
            WeaponMagazine m = magazines[0];
            InventarySystem.Instance.RemoveConcreteMagazine(magazines[0]);
            currentWeapon.Reload(m);
        }
    }
    public void SetAimState(bool state)
    {
        if(currentWeapon != null)
        {
            AimValueChanged?.Invoke(state);
            currentWeapon.SetAimState(state);
        }
    }

    private void OnRecoil(Vector2 recoilVector)
    {
        RecoilEvent?.Invoke(recoilVector);
    }

    private void Update()
    {
        if(reloadTime < 0) return;

        reloadTime += Time.deltaTime;
        if(reloadTime > 1)
        {
            reloadTime = -1;
            if (currentWeapon == null) return;
            if (currentWeapon.reload) return;
            if (currentWeapon.currentMagazine == null) return;

            InventarySystem.Instance.AddConcreteMagazine(currentWeapon.currentMagazine);
            currentWeapon.currentMagazine = null;
            currentWeapon.PullOutMagazine();
        }
    }

    private void OnCurrentWeaponAmmoChanged()
    {
        AmmoChanged?.Invoke(currentWeapon);
    }

    private void OnFinalReload()
    {
        MagazinesForCurrentWeaponChanged?.Invoke(
            InventarySystem.Instance.GetMagazinesOfType(currentWeapon.weaponData.MagazineType),
            currentWeapon);
    }

    private void OnInventaryMagazinesChanged()
    {
        if(currentWeapon == null) return;

        List<WeaponMagazine> weaponMagazines =
            InventarySystem.Instance.GetMagazinesOfType(currentWeapon.weaponData.MagazineType);

        MagazinesForCurrentWeaponChanged?.Invoke(weaponMagazines, currentWeapon);
    }
}
