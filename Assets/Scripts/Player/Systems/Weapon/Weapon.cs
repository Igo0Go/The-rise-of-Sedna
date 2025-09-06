using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int damage = 1;
    [SerializeField, Min(1)]
    private float distance = 1;
    [SerializeField, Min(1)]
    private float bulletSpeed = 1;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private LayerMask ignoreMask;

    public void AttackInput()
    {
        Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation).GetComponent<Bullet>();

        Vector3 targetPoint;

        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo, 
            distance, ~ignoreMask))
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = cameraTransform.position + cameraTransform.forward * distance;
        }

        bullet.transform.forward = targetPoint - bullet.transform.position;
        bullet.LaunchBullet(damage, distance, bulletSpeed, ignoreMask);
    }
}
