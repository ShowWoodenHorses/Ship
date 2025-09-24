using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyWakeParticles : MonoBehaviour
    {
        [Header("Ссылки")]
        public ParticleSystem wakeParticles; // сюда перетащи твой ParticleSystem в инспекторе
        private NavMeshAgent agent;

        [Header("Настройки зависимости")]
        public float minSpeedThreshold = 0.1f;  // ниже этой скорости эффект отключается
        public float maxShipSpeed = 10f;        // при такой скорости эффект максимальный
        public float minSimulationSpeed = 0.5f; // минимальная скорость анимации партиклов
        public float maxSimulationSpeed = 2f;   // максимальная скорость анимации партиклов

        private ParticleSystem.MainModule mainModule;
        private ParticleSystem.EmissionModule emissionModule;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            if (wakeParticles != null)
            {
                mainModule = wakeParticles.main;
                emissionModule = wakeParticles.emission;
            }
        }

        void Update()
        {
            if (wakeParticles == null) return;

            float currentSpeed = agent.velocity.magnitude;

            if (currentSpeed < minSpeedThreshold)
            {
                // отключаем партиклы, если корабль стоит
                if (wakeParticles.isPlaying)
                    wakeParticles.Stop();
            }
            else
            {
                // включаем, если выключены
                if (!wakeParticles.isPlaying)
                    wakeParticles.Play();

                // нормализуем скорость [0..1]
                float t = Mathf.Clamp01(currentSpeed / maxShipSpeed);

                // регулируем скорость анимации партиклов
                mainModule.simulationSpeed = Mathf.Lerp(minSimulationSpeed, maxSimulationSpeed, t);

                // можно ещё emission rate подстроить (опционально)
                emissionModule.rateOverTime = Mathf.Lerp(5f, 50f, t);
            }
        }
    }
}
