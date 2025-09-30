using Assets.Scripts.Interface;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts.Control
{
    public class MobileShipInput : IShipInput
    {
        private Joystick joystick;

        public MobileShipInput(Joystick joystick)
        {
            this.joystick = joystick;
        }

        public float GetAcceleration()
        {
            return joystick.Vertical > 0 ? joystick.Vertical : 0f;
        }

        public float GetBrake()
        {
            return joystick.Vertical < 0 ? -joystick.Vertical : 0f;
        }

        public float GetTurn()
        {
            return joystick.Horizontal;
        }

        public bool IsPointerOverUI()
        {
            if (Input.touchCount > 0)
                return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            return false;
        }
    }
}