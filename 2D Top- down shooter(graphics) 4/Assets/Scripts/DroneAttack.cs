using UnityEngine;

public class DroneAttack : BaseAttacker
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float bulletCooldown = 2f;

    public override float FireRate => fireRate;
    public override float AttackRange => attackRange;
    public override float BulletCooldown => bulletCooldown;
    public override GameObject BulletPrefab => bulletPrefab;
}
