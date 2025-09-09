using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject decal;

    private LayerMask ignoreMask;
    private int damage = 1;
    private float speed = 1;
    private Vector3 oldPos;

    public void LaunchBullet(int damage, float distance, float bulletSpeed, LayerMask ignoreMask)
    {
        float time = distance / bulletSpeed;
        Destroy(gameObject, time);
        this.damage = damage;
        this.ignoreMask = ignoreMask;
        this.speed = bulletSpeed;
        oldPos = transform.position;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if(Physics.Linecast(oldPos, transform.position, out RaycastHit hitInfo, ~ignoreMask))
        {
            if(hitInfo.collider.TryGetComponent(out EnemyBase enemy)) 
            {
                enemy.GedDamage(damage);
            }
            else
            {
                GameObject decalObj = Instantiate(decal, hitInfo.point, Quaternion.identity);
                decalObj.transform.forward = hitInfo.normal;
            }
            Destroy(gameObject);
        }

        oldPos = transform.position;
    }
}
