using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ShopItemConfig", menuName = "Shop/ShopItemConfig")]
    public class ShopItemConfig : ScriptableObject
    {
        public string nameItemText;
        public string nameItemText_EN;
        public string nameItemText_TR;
        public Sprite iconItem;

        public string idItem;
        public int costItem;

        [TextArea]
        public string description;
        public string description_EN;
        public string description_TR;
    }
}