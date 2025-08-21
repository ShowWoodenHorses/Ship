
namespace Assets.Scripts.UI.Shop
{
    public class ShopShipController : ShopController
    {
        public override void UpdateItem(string id)
        {
            shipManager.UpgradeShip(id);
        }
    }
}