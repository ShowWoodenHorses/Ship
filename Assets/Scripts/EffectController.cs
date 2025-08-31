using System.Collections;
using Assets.Scripts.ObjectPool;
using UnityEngine;

namespace Assets.Scripts
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField] private float lifeBeforeDestroy;

        private GameObject refObj;

        public void Initialize(GameObject obj)
        {
            refObj = obj;
            StartCoroutine(LifeBeforeDestroy());
        }

        private void Deactive()
        {
            EffectObjectPool.Instance.ReturnObject(refObj);
            Debug.Log("return: " + refObj);
        }

        IEnumerator LifeBeforeDestroy()
        {
            yield return new WaitForSeconds(lifeBeforeDestroy);
            Deactive();
        }
    }
}