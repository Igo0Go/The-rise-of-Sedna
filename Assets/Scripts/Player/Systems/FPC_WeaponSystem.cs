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

    private Dictionary<MagazineType, List<WeaponMagazine>> magazines = new();

    private event Action<Vector2> RecoilEvent;
    public event Action<Weapon> WeaponChanged;
    public event Action<Weapon> AmmoChanged;
    public event Action<List<WeaponMagazine>, Weapon> MagazinesChanged;

    private float reloadTime = 0;

    private void Awake()
    {
        RecoilEvent += FindFirstObjectByType<FPC_View>().OnRecoil;
        reloadTime = -1;
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

            WeaponItem item = Instantiate(currentWeapon.weaponData.weaponItem, cameraTransform.position + cameraTransform.forward,
                Quaternion.identity).GetComponent<WeaponItem>();

            item.SetMagazine(currentWeapon.currentMagazine);
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = Instantiate(weapon.weaponItemData.weaponPrefab, weaponPoint).GetComponent<Weapon>();
        currentWeapon.Init(cameraTransform, weapon.magazine);

        currentWeapon.Recoil += OnRecoil;
        currentWeapon.AmmoChanged += OnCurrentWeaponAmmoChanged;
        currentWeapon.ReloadFinaled += OnFinalReload;

        if(!magazines.ContainsKey(weapon.weaponItemData.MagazineType))
        {
            magazines.Add(weapon.weaponItemData.MagazineType, new List<WeaponMagazine>());
        }


        WeaponChanged?.Invoke(currentWeapon);
        AmmoChanged?.Invoke(currentWeapon);
        MagazinesChanged?.Invoke(magazines[weapon.weaponItemData.MagazineType], currentWeapon);
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

        if (currentWeapon == null) return;

        if(currentWeapon.weaponData.MagazineType == magazine.data.type)
        {
            MagazinesChanged?.Invoke(magazines[magazine.data.type], currentWeapon);
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
                if(currentWeapon.currentMagazine != null && currentWeapon.currentMagazine.currentAmmo != 0)
                {
                    magazines[currentWeapon.weaponData.MagazineType].Add(currentWeapon.currentMagazine);
                }
                WeaponMagazine m = magazines[currentWeapon.weaponData.MagazineType][0];
                magazines[currentWeapon.weaponData.MagazineType].RemoveAt(0);
                currentWeapon.Reload(m);
            }
        }
    }

    private void OnRecoil(Vector2 recoilVector)
    {
        RecoilEvent?.Invoke(recoilVector);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            string result = string.Empty;

            if (currentWeapon != null && currentWeapon.currentMagazine != null)
            {
                result += "==[" + currentWeapon.currentMagazine.currentAmmo + "]\t";
            }

            if (magazines.ContainsKey(currentWeapon.weaponData.MagazineType))
            {
                foreach(var item in magazines[currentWeapon.weaponData.MagazineType])
                {
                    result += "[" + item.currentAmmo + "] ";
                }
            }

            print(result);
        }

        if(reloadTime < 0) return;

        reloadTime += Time.deltaTime;
        if(reloadTime > 1)
        {
            reloadTime = -1;
            if (currentWeapon == null) return;
            if (currentWeapon.reload) return;
            if (currentWeapon.currentMagazine == null) return;

            AddMagazine(currentWeapon.currentMagazine);
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
        MagazinesChanged?.Invoke(magazines[currentWeapon.weaponData.MagazineType], currentWeapon);
    }
}
