using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{

    float timer;
    [SerializeField] float timeBtwAttack, attackSpeed;
    [SerializeField] int damage;
    [SerializeField] AudioClip attackClip;// Убедитесь, что аудиоклип назначен в инспекторе Unity

    private AudioSource audioSource_; // Добавляем компонент AudioSource

    public override void Start()
    {
        base.Start();
        audioSource_ = GetComponent<AudioSource>(); // Получаем компонент AudioSource
        timer = timeBtwAttack;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (CheckIfCanAttack() && player)
        {
            if(timer >= timeBtwAttack)
            {
                timer = 0;

                StartCoroutine(nameof(Attack));
            }
        }
    }

    IEnumerator Attack()
    {
        Player.Instance.Damage(damage);

        audioSource_.PlayOneShot(attackClip); // Используем AudioSource для воспроизведения звука


        Vector2 origPos = transform.position;
        Vector2 plPos = Player.Instance.transform.position;

        float percent = 0f;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

            transform.position = Vector2.Lerp(origPos, plPos, interpolation);
            yield return null;
        }
    }
}
