using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Shop;
using NUnit.Framework.Interfaces;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private ShipManager shipManager;

        [SerializeField] private ShopShipConfig[] shopShipConfigs;
        [SerializeField] private List<string> avaliableItems;
        [SerializeField] private List<GameObject> allItems;
        [SerializeField] private GameObject prefabShopItem;
        [SerializeField] private Transform parentPosition;

        [SerializeField] private string currentIdItem;

        [SerializeField] private GameObject noMoneyObj;

        private void Start()
        {
            CreateShopItems();
        }

        private void UpdateAvaliableItems()
        {
            foreach(var item in allItems)
            {
                var data = item.GetComponent<ShopItemData>();
                foreach(var id in avaliableItems)
                {
                    if (data.idItem == currentIdItem) continue;

                    if (data.idItem == id)
                    {
                        data.UpdateButtons(data.SelectButton);
                        continue;
                    }

                }
            }
        }

        private void CreateShopItems()
        {
            for (int i = 0; i < shopShipConfigs.Length; i++)
            {
                ShopShipConfig config = shopShipConfigs[i];

                GameObject newItem = Instantiate(prefabShopItem, parentPosition);
                ShopItemData shopItemData = newItem.GetComponent<ShopItemData>();

                if (shopItemData == null) continue;

                shopItemData.Initialize(config);

                shopItemData.OnBuyItem += PurchaseHandler;
                shopItemData.OnSelectItem += ChoiceItemHandler;

                allItems.Add(newItem);
            }
        }

        private void PurchaseHandler(ShopItemData itemData)
        {
            Debug.Log(itemData.idItem);

            if (scoreManager.GetCurrentMoney() < itemData.costItem)
            {
                noMoneyObj.SetActive(true);
                return;
            }

            scoreManager.RemoveMoney(itemData.costItem);
            itemData.UpdateButtons(itemData.SelectButton);
            avaliableItems.Add(itemData.idItem);
        }

        private void ChoiceItemHandler(ShopItemData itemData)
        {
            Debug.Log(itemData.idItem);

            if (avaliableItems.Contains(itemData.idItem))
            {
                shipManager.UpgradeShip(itemData.idItem);
                currentIdItem = itemData.idItem;
                itemData.UpdateButtons(itemData.SelectItemText);
                UpdateAvaliableItems();
            }
        }

        private void OnDestroy()
        {
            foreach(var item in allItems)
            {
                var data = item.GetComponent<ShopItemData>();
                if (data != null)
                {
                    data.OnSelectItem -= ChoiceItemHandler;
                    data.OnBuyItem -= PurchaseHandler;
                }
            }
        }
    }
}