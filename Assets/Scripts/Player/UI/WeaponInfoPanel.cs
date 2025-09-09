using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponInfoPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject infoPnel;
    [SerializeField]
    private TMP_Text weaponNameText;
    [SerializeField]
    private TMP_Text weaponAmmoText;
    [SerializeField]
    private TMP_Text magazinesText;

    void Awake()
    {
        FPC_WeaponSystem ws = FindFirstObjectByType<FPC_WeaponSystem>();
        ws.WeaponChanged += OnWeaponChanged;
        ws.AmmoChanged += OnAmmoChanged;
        ws.MagazinesChanged += OnMagazinesChenged;

        infoPnel.SetActive(false);
    }

    private void OnWeaponChanged(Weapon newWeapon)
    {
        if(newWeapon == null)
        {
            infoPnel.SetActive(false);
        }
        else
        {
            infoPnel.SetActive(true);
            weaponNameText.text = newWeapon.weaponData.Name;
        }
    }
    private void OnAmmoChanged(Weapon currentWeapon)
    {
        if(currentWeapon.currentMagazine == null)
        {
            weaponAmmoText.text = "разряжено";
        }
        else
        {
            int availableShots = currentWeapon.currentMagazine.currentAmmo / currentWeapon.weaponData.consumptionPerShot;
            int maxShots = currentWeapon.currentMagazine.data.maxAmmo / currentWeapon.weaponData.consumptionPerShot;
            weaponAmmoText.text = "[" + availableShots + "/" + maxShots + "]";
        }
    }
    private void OnMagazinesChenged(List<WeaponMagazine> newMagazines, Weapon currentWeapon)
    {
        if(newMagazines == null ||  newMagazines.Count == 0)
        {
            magazinesText.text = string.Empty;
            return;
        }

        string s = string.Empty;

        foreach (var item in newMagazines)
        {
            int availableShots = item.currentAmmo / currentWeapon.weaponData.consumptionPerShot;
            s += "[" + availableShots + "] ";
        }

        magazinesText.text = s;
    }
}
