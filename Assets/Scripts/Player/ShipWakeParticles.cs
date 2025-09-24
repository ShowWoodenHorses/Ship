using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class ShipWakeParticles : MonoBehaviour
    {
        [Header("Ссылки")]
        public ParticleSystem wakeParticles; // сюда в инспекторе перетащи твой ParticleSystem
        private ShipMovement shipMovement;

        [Header("Настройки зависимости")]
        public float minSpeedThreshold = 0.1f;   // ниже этой скорости эффект отключается
        public float minSimulationSpeed = 0.5f;  // минимальная скорость анимации
        public float maxSimulationSpeed = 2f;    // максимальная скорость анимации
        public float minEmissionRate = 5f;       // минимальное количество частиц
        public float maxEmissionRate = 500f;      // максимальное количество частиц

        private ParticleSystem.MainModule mainModule;
        private ParticleSystem.EmissionModule emissionModule;

        public void Initialize(ShipMovement shipMovement)
        {
            this.shipMovement = shipMovement;

            if (wakeParticles != null)
            {
                mainModule = wakeParticles.main;
                emissionModule = wakeParticles.emission;
            }
        }

        void Update()
        {
            if (wakeParticles == null || shipMovement == null) return;

            float currentSpeed = Mathf.Abs(shipMovement.CurrentSpeed); // получаем текущую скорость

            if (currentSpeed < minSpeedThreshold)
            {
                // выключаем эффект
                if (wakeParticles.isPlaying)
                    wakeParticles.Stop();
            }
            else
            {
                // включаем, если выключено
                if (!wakeParticles.isPlaying)
                    wakeParticles.Play();

                // нормализуем [0..1] относительно макс. скорости
                float t = Mathf.Clamp01(currentSpeed / shipMovement.MaxSpeed);

                // подстраиваем скорость анимации
                mainModule.simulationSpeed = Mathf.Lerp(minSimulationSpeed, maxSimulationSpeed, t);

                // подстраиваем частоту спавна
                emissionModule.rateOverTime = Mathf.Lerp(minEmissionRate, maxEmissionRate, t);
            }
        }
    }

}