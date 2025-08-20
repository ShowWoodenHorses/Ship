using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ShipConfig", menuName = "Ships/ShipConfig")]
    public class ShipConfig : ScriptableObject
    {
        [Header("Идентификаторы")]
        public string id;            // Уникальное имя (например "frigate")
        public string displayName;   // Красивое имя ("Фрегат")

        [Header("Характеристики движения")]
        public float acceleration = 5f;
        public float maxSpeed = 20f;
        public float deceleration = 3f;
        public float turnSpeed = 50f;

        [Header("Характеристики прочности")]
        public int maxHealth = 100;

        [Header("Настройка корабля")]
        public GameObject shipPrefab;   // Префаб с пушками и моделью
    }
}