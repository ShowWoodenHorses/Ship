using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Sound
{
    public class SoundPoolManager : MonoBehaviour
    {
        private static SoundPoolManager instance;
        public static SoundPoolManager Instance { get { return instance; } }

        [System.Serializable]
        public class SoundPool
        {
            public AudioSource prefab; // Префаб с AudioSource
            public int size = 5;
        }

        public List<SoundPool> pools;
        public AudioMixerGroup sfxGroup;

        private Dictionary<AudioSource, Queue<AudioSource>> poolDictionary;

        public void Initialize()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            InitializePools();
        }

        private void InitializePools()
        {
            poolDictionary = new Dictionary<AudioSource, Queue<AudioSource>>();

            foreach (SoundPool pool in pools)
            {
                Queue<AudioSource> queue = new Queue<AudioSource>();
                for (int i = 0; i < pool.size; i++)
                {
                    AudioSource obj = CreateNewAudioSource(pool.prefab);
                    queue.Enqueue(obj);
                }
                poolDictionary.Add(pool.prefab, queue);
            }
        }

        private AudioSource CreateNewAudioSource(AudioSource prefab)
        {
            AudioSource obj = Instantiate(prefab, transform);
            obj.playOnAwake = false;
            if (sfxGroup != null)
                obj.outputAudioMixerGroup = sfxGroup;

            obj.gameObject.SetActive(false);
            return obj;
        }

        /// <summary>
        /// Взять объект из пула и проиграть
        /// </summary>
        public void PlaySound(AudioSource prefab, float volume = 1f, float pitch = 1f)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                Debug.LogWarning("SoundPoolManager: Пул для префаба " + prefab.name + " не найден!");
                return;
            }

            AudioSource source;
            if (poolDictionary[prefab].Count == 0)
            {
                // если пул пуст, создаём новый объект
                source = CreateNewAudioSource(prefab);
            }
            else
            {
                source = poolDictionary[prefab].Dequeue();
            }

            source.gameObject.SetActive(true);
            source.volume = volume;
            source.pitch = pitch;
            source.Play();

            StartCoroutine(ReturnToPoolAfterPlay(prefab, source));
        }

        private IEnumerator ReturnToPoolAfterPlay(AudioSource prefab, AudioSource source)
        {
            yield return new WaitWhile(() => source.isPlaying);

            source.Stop();
            poolDictionary[prefab].Enqueue(source);
        }
    }
}
