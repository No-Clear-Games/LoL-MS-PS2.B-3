using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryItemButton : MonoBehaviour
    {

        [SerializeField] private Image iconImage; //TODO: Do something for icon
        [SerializeField] private TMP_Text remainingCountText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button itemBtn;
        [SerializeField] private Transform objectParent;


        private InventorySupply _inventorySupply;

        public event Action<string> ButtonClicked;


        public void Initialize(InventorySupply supply)
        {
            _inventorySupply = supply;
            SetCount(supply.count);
            nameText.text = supply.item.name;
            itemBtn.onClick.AddListener(OnItemBtnClicked);
            GameObject iconObject = Instantiate(supply.item, objectParent);
            iconObject.transform.localPosition = Vector3.zero;
            iconObject.layer = LayerMask.NameToLayer("UI");
        }

        private void OnItemBtnClicked()
        {
            ButtonClicked?.Invoke(_inventorySupply.GetItemId());
        }
        
        

        public void SetCount(uint count)
        {
            remainingCountText.text = count.ToString();
        }
    }
}
