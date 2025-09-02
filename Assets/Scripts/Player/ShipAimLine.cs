using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class ShipAimLine : MonoBehaviour
    {
        public float maxDistance = 100f; // Длина линии, если мышь далеко
        public float startWidth = 1f; // Длина линии, если мышь далеко
        public float endWidth = 1f; // Длина линии, если мышь далеко
        private LineRenderer lr;

        public void Initialize()
        {
            lr = GetComponent<LineRenderer>();
            if (lr == null) lr = gameObject.AddComponent<LineRenderer>();

            lr.positionCount = 2;
            lr.startWidth = startWidth;
            lr.endWidth = endWidth;
            lr.material = new Material(Shader.Find("Sprites/Default"));

            // Градиент прозрачности: начало почти прозрачное, конец полностью видимое
            Gradient grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.2f, 0f), new GradientAlphaKey(1f, 1f) }
            );
            lr.colorGradient = grad;

            lr.enabled = false;
        }

        public void DrawLine(Vector3 start, Vector3 end, bool visible)
        {
            if (!visible)
            {
                lr.enabled = false;
                return;
            }

            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lr.enabled = true;
        }

        public void Hide() => lr.enabled = false;
    }
}