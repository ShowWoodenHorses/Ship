using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "DefaultSaveConfig", menuName = "ScriptableObject/DefaultSaveConfig")]
    public class DefaultSaveConfig : ScriptableObject
    {
        [Header("Стартовые значения")]
        public int currentCoins = 0;
        public int allCoins = 0;
        public string selectedShipId = "ship_basic";
        public string selectedBulletId = "bullet_basic";
        public string currentWaveEnemyId = "wave_1";
        public List<string> ownedItems = new() { "ship_basic", "bullet_basic" };
    }
}