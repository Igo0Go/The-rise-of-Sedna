using UnityEngine;

[CreateAssetMenu(menuName ="IgoGoTools/Items/Weapon")]
public class WeaponItemData : ScriptableObject
{
    public string Name;
    public string ActionDescription;

    [Tooltip("���������� ������� � ��������"), Min(1)]
    public int magazineSize = 1;
    [Tooltip("���� �� ������ �������"), Min(1)]
    public int damage = 1;
    [Tooltip("��������� ��������"), Min(1)]
    public float distance = 1;
    [Tooltip("�������� ����� ����"), Min(1)]
    public float bulletSpeed = 1;
    [Tooltip("������ ����")]
    public GameObject bulletPrefab;
    [Tooltip("����� ����, ������� ������ ���������")]
    public LayerMask ignoreMask;
    [Tooltip("���� ��������")]
    public AudioClip shootCLip;
    [Tooltip("���� ������� ��������")]
    public AudioClip noAmmoCLip;
    [Tooltip("���������������� (��������� � ������)"), Min(1)]
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
