using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Configs;
using System;

namespace Assets.Scripts.UI.Shop
{
    public class ShopItemData : MonoBehaviour
    {
        public TextMeshProUGUI nameItemText;
        public TextMeshProUGUI costItemText;

        public string idItem;
        public int costItem;

        public Image iconItem;

        public GameObject BuyButton;
        public GameObject SelectButton;
        public GameObject SelectItemText;
        public List<GameObject> listButtons;

        public Action<ShopItemData> OnBuyItem;
        public Action<ShopItemData> OnSelectItem;

        private void Awake()
        {
            listButtons.Add(BuyButton);
            listButtons.Add(SelectButton);
            listButtons.Add(SelectItemText);
        }

        public void Initialize(ShopItemConfig shopItemConfig)
        {
            this.idItem = shopItemConfig.idItem;
            this.nameItemText.text = shopItemConfig.nameItemText;
            this.costItemText.text = shopItemConfig.costItem.ToString();
            this.costItem = shopItemConfig.costItem;
            this.iconItem.sprite = shopItemConfig.iconItem;
            UpdateButtons(BuyButton);

            BuyButton.GetComponent<Button>().onClick.AddListener(() => BuyClick());
            SelectButton.GetComponent<Button>().onClick.AddListener(() =>  SelectClick());
        }

        public void BuyClick()
        {
            OnBuyItem?.Invoke(this);
        }

        public void SelectClick()
        {
            OnSelectItem?.Invoke(this);
        }

        public void UpdateButtons(GameObject button)
        {
            foreach (var btn in listButtons)
            {
                btn.SetActive(false);
            }
            button.SetActive(true);
        }
    }
}