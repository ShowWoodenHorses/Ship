using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;      // Корабль или объект, за которым следовать
    public Vector3 offset;        // Смещение относительно цели
    public Quaternion offsetRotation;        // Смещение относительно цели

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = offsetRotation;
        }
    }
}
