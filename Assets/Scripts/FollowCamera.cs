using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // Корабль
    public Vector3 offset;   // Смещение
    public Quaternion offsetRotation;


    [Header("Ограничение")]
    public float minX, maxX, minZ, maxZ;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // Ограничиваем позицию камеры
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);

            transform.position = desiredPosition;
            transform.rotation = offsetRotation;
        }
    }
}
