using System;
using System.Collections.Generic;
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
            DisablePhysics(iconObject);
            iconObject.transform.localScale = Vector3.one;
            iconObject.transform.localPosition = Vector3.zero;
            iconObject.layer = LayerMask.NameToLayer("UI");
        }

        private void OnItemBtnClicked()
        {
            ButtonClicked?.Invoke(_inventorySupply.GetItemId());
        }

        private void DisablePhysics(GameObject obj)
        {
            List<Component> list = new List<Component>();
            obj.GetComponents(list);
            foreach (Component component in list)
            {
                if (!(component is Transform) && !(component is Renderer) && !(component is MeshFilter))
                {
                    Destroy(component);
                }
            }
        }

        public void SetCount(uint count)
        {
            remainingCountText.text = count.ToString();
        }
    }
}
