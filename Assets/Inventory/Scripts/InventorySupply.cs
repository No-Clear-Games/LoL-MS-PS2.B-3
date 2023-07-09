using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory
{
    [Serializable]
    public class InventorySupply
    {
        public GameObject item;
        public uint count;

        public static string GetItemId(GameObject item)
        {
            return item.GetHashCode().ToString();
        }
        
        public string GetItemId()
        {
            return InventorySupply.GetItemId(item);
        }
        
        

        public InventorySupply(InventorySupply i)
        {
            item = i.item;
            count = i.count;
        }
        
        public InventorySupply(GameObject item, uint count)
        {
            this.item = item;
            this.count = count;
        }
    }
}
