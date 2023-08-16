namespace Inventory.Scripts
{
    public interface ICollectible
    {
        public void SetInventoryId(string id);
        public string GetInventoryId();

        public bool HasInventoryId();

    }
}
