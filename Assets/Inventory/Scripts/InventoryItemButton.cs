using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryItemButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField] private Image iconImage; //TODO: Do something for icon
        [SerializeField] private TMP_Text remainingCountText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button itemBtn;
        [SerializeField] private Transform objectParent;
        [SerializeField] private float minHoldSeconds = 2;


        private InventorySupply _inventorySupply;
        private DateTime _pointerDownDateTime;
        // public event Action<string> ButtonClicked;

        public InventorySupply InventorySupply => _inventorySupply;

        public event Action<InventoryItemButton> PointerDownAction;
        public event Action<InventoryItemButton> PointerUpAction;


        public void Initialize(InventorySupply supply)
        {
            _inventorySupply = supply;
            SetCount(supply.count);
            nameText.text = GetNameText(supply);
            // itemBtn.onClick.AddListener(OnItemBtnClicked);
            GameObject iconObject = Instantiate(supply.item, objectParent);
            DisablePhysics(iconObject);
            iconObject.transform.localScale = Vector3.one;
            iconObject.transform.localPosition = Vector3.zero;
            iconObject.layer = LayerMask.NameToLayer("UI");
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("UI");
            }
        }

        private string GetNameText(InventorySupply supply)
        {
            string itemName = "";
            
#if UNITY_EDITOR
            itemName = supply.item.name;
#elif UNITY_WEBGL
            string langKey = supply.item.name.ToLower().Replace(' ', '-');
            itemName = SharedState.LanguageDefs[langKey];
#endif

            return itemName;

        }
        
        // private void OnItemBtnClicked()
        // {
        //     ButtonClicked?.Invoke(_inventorySupply.GetItemId());
        // }

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
        
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDownDateTime = DateTime.Now;
            PointerDownAction?.Invoke(this);
            Debug.Log("Pointer Down");
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if ((DateTime.Now - _pointerDownDateTime).Seconds < minHoldSeconds)
            {
                return;
            }
            PointerUpAction?.Invoke(this);
            Debug.Log("Pointer Up");
        }
    }
}
