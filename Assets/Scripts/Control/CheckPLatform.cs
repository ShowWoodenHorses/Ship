using Assets.Scripts.Interface;
using UnityEngine;
using YG;

namespace Assets.Scripts.Control
{
    public class CheckPLatform : MonoBehaviour
    {
        public Joystick mobileJoystick;
        public GameObject joystickUI;

        public IShipInput CheckCurrentPlatform()
        {
            //Проверить как мобилку
            //joystickUI.SetActive(true);
            //return new MobileShipInput(mobileJoystick);
            //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            //{
            //    joystickUI.SetActive(true);
            //    return new MobileShipInput(mobileJoystick);
            //}
            //else
            //{
            //    joystickUI.SetActive(false);
            //    return new PCShipInput();
            //}

            if (YG2.envir.isMobile)
            {
                joystickUI.SetActive(true);
                return new MobileShipInput(mobileJoystick);
            }
            else
            {
                joystickUI.SetActive(false);
                return new PCShipInput();
            }
        }
    }
}