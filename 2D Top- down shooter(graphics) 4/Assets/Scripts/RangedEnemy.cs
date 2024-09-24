using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    float timer;
    [SerializeField] float timeBtwAttack;
    [SerializeField] Transform shotPos;
    new protected Transform player; // скрывает поле player в родительском классе
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip attackClip; // Аудиоклип для звука атаки

    private AudioSource audioSource; // Добавляем поле для компонента AudioSource

    public override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>(); // Инициализируем компонент AudioSource
        timer = timeBtwAttack;
        player = Player.Instance.transform;
    }

    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (CheckIfCanAttack() && player)
        {
            if (timer >= timeBtwAttack)
            {
                timer = 0;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip); // Воспроизводим звук атаки
        }
        Vector2 direction = player.position - shotPos.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotPos.rotation = rotation;
        Instantiate(bullet, shotPos.position, shotPos.rotation);
    }
}
