using UnityEngine;

public class DamagebleObject : MonoBehaviour
{
    [SerializeField]
    private int HP;
    [SerializeField]
    private GameObject basePart;
    [SerializeField]
    private GameObject crushedPart;

    public void GetDamage(int damage)
    {
        if (HP > 0)
        {
            HP -= damage;
            if (HP <= 0)
            {
                basePart.SetActive(false);
                crushedPart.SetActive(true);
            }
        }
    }
}
