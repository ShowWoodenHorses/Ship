using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Shop
{
    public class ShopShipData : MonoBehaviour
    {
        [SerializeField] private string nameItem;
        [SerializeField] private string idItem;
        [SerializeField] private Image iconItem;
        [SerializeField] private int costItem;

        [SerializeField] private GameObject BuyButton;
        [SerializeField] private GameObject SelectButton;
    }
}