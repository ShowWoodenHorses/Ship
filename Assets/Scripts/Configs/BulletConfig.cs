using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "Ships/BulletConfig")]
    public class BulletConfig : ScriptableObject
    {
        [Header("Айди")]
        public string id;

        [Header("Характеристики")]
        public float speed;
        public int damageEnemy = 5;
        public int damageBuilding = 5;
        public float lifeBeforeDestroy;

        [Header("Префаб")]
        public GameObject bulletPrefab;
        public AudioSource soundShotPrefab;

    }
}