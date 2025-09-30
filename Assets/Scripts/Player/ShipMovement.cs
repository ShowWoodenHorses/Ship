using Assets.Scripts.Control;
using Assets.Scripts.Interface;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    private float acceleration = 5f;
    private float maxSpeed = 20f;
    private float deceleration = 3f;
    private float turnSpeed = 50f;

    private float currentSpeed = 0f;
    private Rigidbody rb;

    private IShipInput shipInput;

    public float CurrentSpeed => currentSpeed;
    public float MaxSpeed => maxSpeed;

    public void Initialize(float acceleration, float maxSpeed, float deceleration, float turnSpeed, IShipInput shipInput)
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        this.acceleration = acceleration;
        this.maxSpeed = maxSpeed;
        this.deceleration = deceleration;
        this.turnSpeed = turnSpeed;

        this.shipInput = shipInput;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        if (shipInput == null) return;

        float accel = shipInput.GetAcceleration();
        float brake = shipInput.GetBrake();

        if (accel > 0)
            currentSpeed += acceleration * accel * Time.deltaTime;
        else if (brake > 0)
            currentSpeed -= deceleration * brake * Time.deltaTime;
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);

        Vector3 move = transform.forward * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    void HandleRotation()
    {
        if (shipInput == null) return;

        float turn = shipInput.GetTurn();
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
