using Assets.Scripts.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Control
{
    public class PCShipInput : IShipInput
    {
        public float GetAcceleration()
        {
            return Input.GetKey(KeyCode.W) ? 1f : 0f;
        }

        public float GetBrake()
        {
            return Input.GetKey(KeyCode.S) ? 1f : 0f;
        }

        public float GetTurn()
        {
            if (Input.GetKey(KeyCode.A)) return -1f;
            if (Input.GetKey(KeyCode.D)) return 1f;
            return 0f;
        }

        public bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}