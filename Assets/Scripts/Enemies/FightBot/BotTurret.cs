using System.Collections;
using UnityEngine;
using System;

public class BotTurret : EnemyPart
{
    [SerializeField]
    private Transform shootPoint;

    [SerializeField]
    private LayerMask ignoreMask;
    [SerializeField]
    private AudioSource shootSource;
    [SerializeField]
    private WeaponItemData weaponData;

    private float shootDelayTime;
    private bool delay = false;
    private bool EnemyOnDistance
    {
        get
        {
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hitInfo, 
                weaponData.distance, ~ignoreMask))
            {
                if(hitInfo.collider.CompareTag("Enemy"))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public event Action DamageEvent;

    private bool CanShoot => !EnemyOnDistance && !delay;

    private void Awake()
    {
        shootDelayTime = 1 / (weaponData.fireRate / 60);
    }

    public void Aim(Vector3 target)
    {
        transform.forward = target - transform.position;
    }

    public void TryShoot()
    {
        if (CanShoot)
        {
            SpawnBullet();
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        delay = true;
        yield return new WaitForSeconds(shootDelayTime);
        delay = false;
    }

    private void SpawnBullet()
    {
        Bullet bullet = Instantiate(weaponData.bulletPrefab, shootPoint.position, shootPoint.rotation).
            GetComponent<Bullet>();

        bullet.LaunchBullet(weaponData.damage, weaponData.distance, weaponData.bulletSpeed, weaponData.ignoreMask);
        AudioPack.audioSystem.PlaySound(weaponData.shootCLip);
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        DamageEvent?.Invoke();
    }
}
