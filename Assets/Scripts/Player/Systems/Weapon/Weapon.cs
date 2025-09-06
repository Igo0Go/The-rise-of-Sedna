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
    private LayerMask ignoreMask;
    [SerializeField]
    private AudioClip shootCLip;
    [SerializeField]
    private float fireRate = 1;

    private Transform cameraTransform;
    private float shootDelay;
    private bool shoot;
    private float currentTime;

    public void Init(Transform camera)
    {
        cameraTransform = camera;
        shootDelay = 1 / (fireRate / 60);
    }

    public void AttackInput()
    {
        shoot = true;
        currentTime = 0;
    }

    public void StopMainAttack()
    {
        shoot = false;
    }

    private void SpawnBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation).GetComponent<Bullet>();

        Vector3 targetPoint;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo,
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
        AudioPack.audioSystem.PlaySound(shootCLip);
    }

    private void Update()
    {
        if(shoot)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                SpawnBullet();
                currentTime = shootDelay;
            }
        }
    }
}
