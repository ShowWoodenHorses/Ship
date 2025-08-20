using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;      // ������� ��� ������, �� ������� ���������
    public Vector3 offset;        // �������� ������������ ����
    public Quaternion offsetRotation;        // �������� ������������ ����

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = offsetRotation;
        }
    }
}
