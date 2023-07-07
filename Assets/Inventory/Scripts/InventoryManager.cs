using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryUiController inventoryUiControllerPrefab;
        [SerializeField] private Canvas inventoryParentCanvas;

        private Dictionary<string, InventorySupply> _inventorySupplies;
        private InventoryUiController _inventoryUiController;


        public event Action<GameObject> AddItemAction; 
        public event Action<GameObject> GetItemAction; 
        public event Action<string> ItemSupplyEnded; 


        public void InitInventory(InventoryConfig inventoryConfig)
        {
            _inventorySupplies = new Dictionary<string, InventorySupply>();
            _inventoryUiController = Instantiate(inventoryUiControllerPrefab.gameObject, inventoryParentCanvas.transform)
                .GetComponent<InventoryUiController>();
            
            _inventoryUiController.Initialize();
            _inventoryUiController.ButtonCreated += InventoryUiControllerOnButtonCreated;
            
            foreach (InventorySupply supply in inventoryConfig.inventorySupplies)
            {
                AddItem(supply);
            }
        }

        private void InventoryUiControllerOnButtonCreated(InventoryItemButton button)
        {
            button.ButtonClicked += GetItem;
        }

        public void AddItem(InventorySupply supply)
        {
            AddItem(supply.item, supply.count);
        }
        
        public void AddItem(GameObject item, uint count)
        {
            string id = InventorySupply.GetItemId(item);
            if (_inventorySupplies.TryGetValue(id, out InventorySupply supply))
            {
                supply.count += count;
                if (_inventoryUiController.TryGetButton(id, out InventoryItemButton button))
                {
                    
                    button.SetCount(supply.count);
                }
            }
            else
            {
                InventorySupply newSupply = new InventorySupply(item, count);
                _inventorySupplies[id] = newSupply;
                _inventoryUiController.CreateItemButton(newSupply);
            }
            
            AddItemAction?.Invoke(item);
            
        }


        private void GetItem(string itemId)
        {
            bool exists = _inventorySupplies.TryGetValue(itemId, out InventorySupply supply);

            if (!exists)
            {
                return ;
            }

            supply.count--;
            
            GetItemAction?.Invoke(supply.item);

            if (supply.count == 0)
            {
                _inventorySupplies.Remove(itemId);
                ItemSupplyEnded?.Invoke(itemId);
            }
        }

    }
}
