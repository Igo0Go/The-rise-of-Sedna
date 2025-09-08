using UnityEngine;

[CreateAssetMenu(menuName ="IgoGoTools/Items/Weapon")]
public class WeaponItemData : ScriptableObject
{
    public string Name;
    public string ActionDescription;

    [Tooltip("Количество зарядов в магазине"), Min(1)]
    public int magazineSize = 1;
    [Tooltip("Урон от одного снаряда"), Min(1)]
    public int damage = 1;
    [Tooltip("Дальность стрельбы"), Min(1)]
    public float distance = 1;
    [Tooltip("Скорость полёта пули"), Min(1)]
    public float bulletSpeed = 1;
    [Tooltip("Объект пули")]
    public GameObject bulletPrefab;
    [Tooltip("Маска слоёв, которые снаряд пробивает")]
    public LayerMask ignoreMask;
    [Tooltip("Звук выстрела")]
    public AudioClip shootCLip;
    [Tooltip("Звук пустого магазина")]
    public AudioClip noAmmoCLip;
    [Tooltip("Скорострельность (выстрелов в минуту)"), Min(1)]
    public float fireRate = 1;
    public ShootMode shootMode = ShootMode.ClicToOneShot;

    public GameObject weaponPrefab;
    public GameObject weaponItem;
}

public enum ShootMode
{
    ClicToOneShot,
    PressToShoot
}
