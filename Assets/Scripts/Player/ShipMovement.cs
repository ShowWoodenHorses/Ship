using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float acceleration = 5f;         // Насколько быстро ускоряется
    public float maxSpeed = 20f;            // Максимальная скорость
    public float deceleration = 3f;         // Насколько быстро тормозит
    public float turnSpeed = 50f;           // Скорость поворота

    private float currentSpeed = 0f;

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }
        else
        {
            // Постепенное замедление, если не нажаты кнопки
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed); // Можно двигаться немного назад
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        float turn = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            turn = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turn = 1f;
        }

        transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);
    }
}
