using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ShipDatabase", menuName = "Ships/ShipDatabase")]
    public class ShipDatabase : ScriptableObject
    {
        public ShipConfig[] ships;

        public ShipConfig GetShipById(string id)
        {
            return ships.FirstOrDefault(s => s.id == id);
        }

        public ShipConfig GetShipByName(string name)
        {
            return ships.FirstOrDefault(s => s.displayName == name);
        }
    }
}