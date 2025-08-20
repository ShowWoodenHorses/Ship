using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float acceleration = 5f;         // ��������� ������ ����������
    public float maxSpeed = 20f;            // ������������ ��������
    public float deceleration = 3f;         // ��������� ������ ��������
    public float turnSpeed = 50f;           // �������� ��������

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
            // ����������� ����������, ���� �� ������ ������
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed); // ����� ��������� ������� �����
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
