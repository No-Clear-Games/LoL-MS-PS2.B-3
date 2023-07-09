using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Inventory
{
    public class InventoryUiController : MonoBehaviour
    {
        [SerializeField] private Transform itemButtonsParent;
        [SerializeField] private InventoryItemButton inventoryItemButtonPrefab;
        [SerializeField] private Camera uiCamera;
        
        private Dictionary<string, InventoryItemButton> _inventoryItemButtons;

        public event Action<InventoryItemButton> ButtonCreated; 
        
        public void Initialize()
        {
            _inventoryItemButtons = new Dictionary<string, InventoryItemButton>();
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiCamera);
        }

        public bool TryGetButton(string itemId, out InventoryItemButton button)
        {
            return _inventoryItemButtons.TryGetValue(itemId, out button);
        }

        public bool CreateItemButton(InventorySupply supply)
        {
            if (_inventoryItemButtons.ContainsKey(supply.GetItemId()))
            {
                return false;
            }

            InventoryItemButton button = Instantiate(inventoryItemButtonPrefab.gameObject, itemButtonsParent).GetComponent<InventoryItemButton>();
            
            button.Initialize(supply);
            _inventoryItemButtons[supply.GetItemId()] = button;

            ButtonCreated?.Invoke(button);
            return true;
        }

        public bool RemoveItemButton(string itemId)
        {
            if (_inventoryItemButtons.TryGetValue(itemId, out InventoryItemButton button))
            {
                Destroy(button.gameObject);
                _inventoryItemButtons.Remove(itemId);
                return true;
            }

            return false;
        }
        
    }
}
