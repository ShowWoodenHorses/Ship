using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BulletDatabase", menuName = "Ships/BulletDatabase")]
    public class BulletDatabase : ScriptableObject
    {
        public BulletConfig[] bulletConfigs;

        public BulletConfig GetBulletById(string id)
        {
            return bulletConfigs.FirstOrDefault(c => c.id == id);
        }
    }
}