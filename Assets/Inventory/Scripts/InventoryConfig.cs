using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "Configs/Inventory")]
    public class InventoryConfig : ScriptableObject
    {
        public List<InventorySupply> inventorySupplies;
    }
}
