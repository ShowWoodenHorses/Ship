using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class DontDestroyOnLoadScene : MonoBehaviour
    {
        private static DontDestroyOnLoadScene Instance = null;
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}