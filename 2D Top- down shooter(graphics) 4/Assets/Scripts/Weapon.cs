using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyGame;

public class Weapon : BaseAttacker
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float bulletCooldown = 0.1f;

    [SerializeField] Transform shootPos;
    [SerializeField] Transform[] shootSuperPos;
    [SerializeField] TextMeshProUGUI text;

    Animator anim;
    SpriteRenderer spR;

    public static Weapon Instance;

    [SerializeField] AudioSource shootAudioSource;
    [SerializeField] SpriteRenderer muzzleFlashSpR;
    [SerializeField] Sprite[] spritesMuzzleFlash;
    [SerializeField] AudioClip[] shootClip, superShootClip;

    AudioSource audS;

    //private float nextFireTime = 0f;

    public override float FireRate => fireRate;
    public override float AttackRange => attackRange;
    public override float BulletCooldown => bulletCooldown;
    public override GameObject BulletPrefab => bulletPrefab;

    private void Awake()
    {
        Instance = this;
        Shop.Instance.buySeconPosition += UpdateFireRate;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        spR = GetComponent<SpriteRenderer>();
        audS = GetComponent<AudioSource>();
        shootAudioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        if (Time.time >= nextFireTime)
        {
            if (CanAttack())
            {
                Attack();
                nextFireTime = Time.time + 1f / FireRate;
            }
        }
    }

    void UpdateFireRate()
    {
        fireRate += 0.1f;
    }

    protected override bool CanAttack()
    {
        return FindNearestEnemy() != null;
    }

    public override void Attack()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            Shoot();
            if (PlayerPrefs.GetInt("Position1") == 1)
            {
                SuperShoot();
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= AttackRange)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void Shoot()
    {
        audS.clip = shootClip[Random.Range(0, shootClip.Length)];
        audS.Play();
        Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        shootAudioSource.Play();
        StartCoroutine(nameof(SetMuzzleFlash));
    }

    void SuperShoot()
    {
        for (int i = 0; i < shootSuperPos.Length; i++)
        {
            Instantiate(bulletPrefab, shootSuperPos[i].position, shootSuperPos[i].rotation);
        }
        SoundManager.Instance.PlayerSound(shootAudioSource.clip);
        CameraFollow.Instance.CamShake();
        StartCoroutine(nameof(SetMuzzleFlash));
    }

    IEnumerator SetMuzzleFlash()
    {
        muzzleFlashSpR.enabled = true;
        muzzleFlashSpR.sprite = spritesMuzzleFlash[Random.Range(0, spritesMuzzleFlash.Length)];
        yield return new WaitForSeconds(0.1f);
        muzzleFlashSpR.enabled = false;
    }
}
