using UnityEngine;
using DG.Tweening;

public class ShipMovement : MonoBehaviour
{
    public float acceleration = 5f;         // Насколько быстро ускоряется
    public float maxSpeed = 20f;            // Максимальная скорость
    public float deceleration = 3f;         // Насколько быстро тормозит
    public float turnSpeed = 50f;           // Скорость поворота

    private float currentSpeed = 0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
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
