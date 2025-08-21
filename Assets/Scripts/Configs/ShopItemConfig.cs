using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ShopItemConfig", menuName = "Shop/ShopItemConfig")]
    public class ShopItemConfig : ScriptableObject
    {
        public string nameItemText;
        public Sprite iconItem;

        public string idItem;
        public int costItem;
    }
}