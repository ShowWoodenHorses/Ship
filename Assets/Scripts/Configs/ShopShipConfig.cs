using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ShopShipConfig", menuName = "Shop/ShopShipConfig")]
    public class ShopShipConfig : ScriptableObject
    {
        public string nameItemText;
        public Sprite iconItem;

        public string idItem;
        public int costItem;
    }
}