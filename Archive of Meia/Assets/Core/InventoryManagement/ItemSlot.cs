
namespace Assets.Core.InventoryManagement
{
    class ItemSlot
    {
        public ItemSlot(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }
        public Item Item { get; set; }
        public int Amount { get; set; }
    }
}
