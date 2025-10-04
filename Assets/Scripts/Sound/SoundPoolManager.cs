using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Sound
{
    public class SoundPoolManager : MonoBehaviour
    {
        public static SoundPoolManager Instance;

        [System.Serializable]
        public class SoundPool
        {
            public AudioSource prefab;
            public int size = 5;
        }

        public List<SoundPool> pools;
        public AudioMixerGroup sfxGroup;

        private Dictionary<AudioSource, Queue<AudioSource>> poolDictionary;

        private AudioMixer mainMixer;
        private const string musicParam = "SFXVolume";

        public void Initialize(AudioMixer mainMixer)
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            this.mainMixer = mainMixer;
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
            obj.volume = 1f; // управление через AudioMixer
            if (sfxGroup != null)
                obj.outputAudioMixerGroup = sfxGroup;

            float startVal;
            mainMixer.GetFloat(musicParam, out startVal);
            startVal = Mathf.Pow(10, startVal / 20f); // перевод dB -> 0..1

            // Фейдим через AudioMixer
            DOTween.To(() => 0.01f,
                       x => mainMixer.SetFloat(musicParam, Mathf.Log10(x) * 20f),
            startVal,
                       1f);

            obj.gameObject.SetActive(true);
            return obj;
        }

        public void PlaySound(AudioSource prefab, float pitch = 1f)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                Debug.LogWarning("SoundPoolManager: Пул для префаба " + prefab.name + " не найден!");
                return;
            }

            AudioSource source;
            if (poolDictionary[prefab].Count == 0)
            {
                source = CreateNewAudioSource(prefab);
            }
            else
            {
                source = poolDictionary[prefab].Dequeue();
            }

            source.gameObject.SetActive(true);
            source.pitch = pitch; // только питч регулируем вручную
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
