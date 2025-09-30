using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Control
{
    public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private RectTransform background;
        private RectTransform handle;

        private Vector2 inputVector;

        [Range(0f, 3f)] public float handleLimit = 1f;

        void Awake()
        {
            background = GetComponent<RectTransform>();
            handle = transform.GetChild(0).GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
            {
                pos.x = (pos.x / background.sizeDelta.x) * 2f;
                pos.y = (pos.y / background.sizeDelta.y) * 2f;

                inputVector = new Vector2(pos.x, pos.y);
                inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

                // Двигаем "ручку"
                handle.anchoredPosition = new Vector2(inputVector.x * (background.sizeDelta.x / 2f) * handleLimit,
                                                      inputVector.y * (background.sizeDelta.y / 2f) * handleLimit);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputVector = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }

        public float Horizontal => inputVector.x;
        public float Vertical => inputVector.y;
        public Vector2 Direction => new Vector2(Horizontal, Vertical);
    }
}