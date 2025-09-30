using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIDisplayCannon : MonoBehaviour
    {
        [SerializeField] private GameObject cannonUIPrefab;

        [Header("Sides")]
        [SerializeField] private Transform leftSideGroup;
        [SerializeField] private Transform rightSideGroup;
        [SerializeField] private Transform frontSideGroup;
        [SerializeField] private Transform rearSideGroup;

        [Header("List")]
        [SerializeField] private List<UICannonPLayer> leftSideList = new List<UICannonPLayer>();
        [SerializeField] private List<UICannonPLayer> rightSideList = new List<UICannonPLayer>();
        [SerializeField] private List<UICannonPLayer> frontSideList = new List<UICannonPLayer>();
        [SerializeField] private List<UICannonPLayer> rearSideList = new List<UICannonPLayer>();

        public void Initialize(ShipCannonMultiSide.CannonSide side, float reloadTime)
        {
            GameObject obj = Instantiate(cannonUIPrefab);
            UICannonPLayer uiCannonPlayer = obj.GetComponent<UICannonPLayer>();
            uiCannonPlayer.Initialize(reloadTime);
            switch (side)
            {
                case ShipCannonMultiSide.CannonSide.Left:
                    leftSideList.Add(uiCannonPlayer);
                    obj.transform.SetParent(leftSideGroup);
                    obj.GetComponent<RectTransform>().localScale = Vector3.one;
                    break;
                case ShipCannonMultiSide.CannonSide.Right:
                    rightSideList.Add(uiCannonPlayer);
                    obj.transform.SetParent(rightSideGroup);
                    obj.GetComponent<RectTransform>().localScale = Vector3.one;
                    break;
                case ShipCannonMultiSide.CannonSide.Front:
                    frontSideList.Add(uiCannonPlayer);
                    obj.transform.SetParent(frontSideGroup);
                    obj.GetComponent<RectTransform>().localScale = Vector3.one;
                    break;
                case ShipCannonMultiSide.CannonSide.Rear:
                    rearSideList.Add(uiCannonPlayer);
                    obj.transform.SetParent(rearSideGroup);
                    obj.GetComponent<RectTransform>().localScale = Vector3.one;
                    break;
            }
        }

        public void UpdateCannons(ShipCannonMultiSide.CannonSide side, int index)
        {
            var allUICannons = GetListFromSide(side);

            for(int i = 0; i < allUICannons.Count; i++)
            {
                if (allUICannons[index])
                {
                    allUICannons[index].SetDisableState();
                }
            }
        }

        public void ClearList()
        {
            UICannonPLayer[] uiCannons = GetComponentsInChildren<UICannonPLayer>(true);

            for (int i = 0; i < uiCannons.Length; i++)
            {
                Destroy(uiCannons[i].gameObject);
            }

            leftSideList.Clear();
            rightSideList.Clear();
            frontSideList.Clear();
            rearSideList.Clear();
        }

        private List<UICannonPLayer> GetListFromSide(ShipCannonMultiSide.CannonSide side)
        {
            switch(side)
            {
                case ShipCannonMultiSide.CannonSide.Left:
                    return leftSideList;
                case ShipCannonMultiSide.CannonSide.Right:
                    return rightSideList;
                case ShipCannonMultiSide.CannonSide.Front:
                    return frontSideList;
                case ShipCannonMultiSide.CannonSide.Rear:
                    return rearSideList;
                default:
                    return null;
            }
        }
    }
}