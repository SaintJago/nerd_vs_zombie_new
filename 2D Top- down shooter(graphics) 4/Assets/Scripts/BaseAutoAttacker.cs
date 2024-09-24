using UnityEngine;

public abstract class BaseAttacker : MonoBehaviour, IAttacker
{
    public abstract float FireRate { get; }
    public abstract float AttackRange { get; }
    public abstract float BulletCooldown { get; }
    public abstract GameObject BulletPrefab { get; }
    public Transform Transform => transform;

    protected float nextFireTime = 0f;
    private bool canShoot = true;

    protected virtual void Update()
    {
        if (Time.time >= nextFireTime)
        {
            if (CanAttack())
            {
                Attack();
                canShoot = false;
                nextFireTime = Time.time + 1f / FireRate;
            }
        }

        if (!canShoot && Time.time >= nextFireTime + BulletCooldown)
        {
            canShoot = true;
        }
    }

    protected virtual bool CanAttack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(Transform.position, enemy.transform.position);
            if (distance <= AttackRange)
            {
                return true;
            }
        }

        return false;
    }

    public virtual void Attack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(Transform.position, enemy.transform.position);
            if (distance <= AttackRange)
            {
                GameObject bullet = Instantiate(BulletPrefab, Transform.position, Quaternion.identity);
                bullet.transform.right = (enemy.transform.position - Transform.position).normalized;
                break;
            }
        }
    }
}
