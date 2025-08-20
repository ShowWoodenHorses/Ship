using Assets.Scripts.Player;
using UnityEngine;

public class TestUpgradePlayer : MonoBehaviour
{
    public ShipManager shipManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            shipManager.UpgradeShip("ship2");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            shipManager.UpgradeShip("ship3");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            shipManager.UpgradeBullet("bullet1");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            shipManager.UpgradeBullet("bullet2");
        }
    }
}
