using UnityEngine;

public class Decal : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float lifeTime = 1;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}
