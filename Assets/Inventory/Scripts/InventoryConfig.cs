using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "Configs/Inventory")]
    public class InventoryConfig : ScriptableObject
    {
        public List<InventorySupply> inventorySupplies;

        public bool GetInventorySupply(string id, out InventorySupply supplyOutput)
        {
            foreach (InventorySupply inventorySupply in inventorySupplies)
            {
                if (inventorySupply.GetItemId() == id)
                {
                    supplyOutput = inventorySupply;
                    return true;
                }
            }

            supplyOutput = null;
            return false;
        }
    }
}
