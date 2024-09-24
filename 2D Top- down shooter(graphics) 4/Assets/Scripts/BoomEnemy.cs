using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEnemy : Enemy
{
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] int damage;
    [SerializeField] GameObject boomEffect;
    [SerializeField] AudioClip attackClip; // Убедитесь, что аудиоклип назначен в инспекторе Unity

    private AudioSource audioSource; // Добавляем компонент AudioSource

    public override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource
    }

    public override void Update()
    {
        base.Update();

        if (CheckIfCanAttack())
        {
            BoomAttack();
        }
    }

    void BoomAttack()
    {
        audioSource.PlayOneShot(attackClip); // Используем AudioSource для воспроизведения звука

        Collider2D[] detectedObject = Physics2D.OverlapCircleAll(transform.position, attackRadius, whatIsPlayer);

        foreach (Collider2D item in detectedObject)
        {
            item?.GetComponent<Player>()?.Damage(damage);
        }

        Instantiate(boomEffect, transform.position, Quaternion.identity);

        Death();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
