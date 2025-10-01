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
        public string description;

        public Image iconItem;

        public GameObject BuyButton;
        public GameObject SelectButton;
        public GameObject SelectItemText;
        public List<GameObject> listButtons;

        public Action<ShopItemData> OnBuyItem;
        public Action<ShopItemData> OnSelectItem;

        private TextMeshProUGUI descriptionItem;

        private void AddButtons()
        {
            listButtons.Add(BuyButton);
            listButtons.Add(SelectButton);
            listButtons.Add(SelectItemText);
        }

        public void Initialize(ShopItemConfig shopItemConfig, TextMeshProUGUI descriptionItem)
        {
            this.idItem = shopItemConfig.idItem;
            this.nameItemText.text = shopItemConfig.nameItemText;
            this.costItemText.text = shopItemConfig.costItem.ToString();
            this.costItem = shopItemConfig.costItem;
            this.iconItem.sprite = shopItemConfig.iconItem;
            this.description = shopItemConfig.description;
            this.descriptionItem = descriptionItem;

            AddButtons();
            UpdateButtons(BuyButton);

            BuyButton.GetComponent<Button>().onClick.AddListener(() => BuyClick());
            SelectButton.GetComponent<Button>().onClick.AddListener(() =>  SelectClick());
            GetComponent<Button>().onClick.AddListener(() => SetDescription());
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

        public void SetDescription()
        {
            descriptionItem.text = description;
        }
    }
}