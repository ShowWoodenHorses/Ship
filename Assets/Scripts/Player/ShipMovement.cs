using UnityEngine;
using DG.Tweening;

public class ShipMovement : MonoBehaviour
{
    private float acceleration = 5f;         // Насколько быстро ускоряется
    private float maxSpeed = 20f;            // Максимальная скорость
    private float deceleration = 3f;         // Насколько быстро тормозит
    private float turnSpeed = 50f;           // Скорость поворота

    private float currentSpeed = 0f;
    private Rigidbody rb;

    public float CurrentSpeed => currentSpeed;
    public float MaxSpeed => maxSpeed;

    public void Initialize(float acceleration, float maxSpeed, float deceleration, float turnSpeed)
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        this.acceleration = acceleration;
        this.maxSpeed = maxSpeed;
        this.deceleration = deceleration;
        this.turnSpeed = turnSpeed;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
            currentSpeed += acceleration * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            currentSpeed -= deceleration * Time.deltaTime;
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);

        Vector3 move = transform.forward * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    void HandleRotation()
    {
        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        else if (Input.GetKey(KeyCode.D)) turn = 1f;

        Quaternion turnRotation = Quaternion.Euler(0f, turn * turnSpeed * Time.deltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentSpeed = 0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        currentSpeed = 0f;
    }
}
