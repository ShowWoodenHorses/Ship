
using System.Collections.Generic;
using Assets.Scripts.Save;

namespace Assets.Scripts.UI.Shop
{
    public class ShopShipController : ShopController
    {

        public void Initialize(List<string> saveAvaliableItems, string currentItemsId)
        {
            enabled = false;
            UpdateAvaliableItems(saveAvaliableItems, currentItemsId);
            base.CreateShopItems();
            enabled = true;
        }
        public override void UpdateItem(string id)
        {
            shipManager.UpgradeShip(id);
            SaveLifecycle.instance.SelectShip(id);
        }

        private void UpdateAvaliableItems(List<string> saveAvaliableItems, string currentItemsId)
        {
            foreach (string item in saveAvaliableItems)
            {
                if (!avaliableItems.Contains(item))
                    avaliableItems.Add(item);
            }

            if (avaliableItems.Contains(currentItemsId))
                currentIdItem = currentItemsId;
        }
    }
}