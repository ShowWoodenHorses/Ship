using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // Корабль
    public Vector3 offset;   // Смещение
    public Quaternion offsetRotation;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            transform.position = desiredPosition;
            transform.rotation = offsetRotation;
        }
    }
}
