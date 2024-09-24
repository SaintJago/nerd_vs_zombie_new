using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public Transform target; // Персонаж
    public float rotationSpeed = 5f;
    public float distance = 2f;

    void Update()
    {
        OrbitAroundTarget();
    }

    void OrbitAroundTarget()
    {
        if (target == null)
            return;

        // Определяем угол вокруг персонажа
        float angle = Time.time * rotationSpeed;

        // Вычисляем новую позицию вокруг персонажа в 2D
        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;

        Vector3 desiredPosition = target.position + new Vector3(x, y, 0f);

        // Плавное перемещение к новой позиции
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationSpeed);

        // Дрон всегда смотрит на персонажа
        transform.up = (target.position - transform.position).normalized;
    }
}
