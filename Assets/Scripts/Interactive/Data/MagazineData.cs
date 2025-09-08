using UnityEngine;

[CreateAssetMenu(fileName = "MagazineData", menuName = "IgoGoTools/Items/Magazines")]
public class MagazineData : ScriptableObject
{
    public MagazineType type;
    [Tooltip("���������� �������� � ��������"), Min(1)]
    public int maxAmmo = 1;
    public string magazineName;
    public string actionDescription;
}
