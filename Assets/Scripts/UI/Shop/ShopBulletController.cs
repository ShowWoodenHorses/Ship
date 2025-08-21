
namespace Assets.Scripts.UI.Shop
{
    public class ShopBulletController : ShopController
    {
        public override void UpdateItem(string id)
        {
            shipManager.UpgradeBullet(id);
        }
    }
}