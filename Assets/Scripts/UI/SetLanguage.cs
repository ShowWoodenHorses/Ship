using System.Collections;
using TMPro;
using UnityEngine;
using YG;

namespace Assets.Scripts.UI
{
    public class SetLanguage : MonoBehaviour
    {
        public string ru, en, tr;

        private TextMeshProUGUI textComponent;

        private void Awake()
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            YG2.onSwitchLang += SwitchLanguage;
            SwitchLanguage(YG2.lang);
        }
        private void OnDisable()
        {
            YG2.onSwitchLang -= SwitchLanguage;
        }

        public void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    textComponent.text = ru;
                    break;
                case "tr":
                    textComponent.text = tr;
                    break;
                default:
                    textComponent.text = en;
                    break;
            }
        }
    }
}