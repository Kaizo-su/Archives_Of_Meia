using System;
using System.Collections;
using System.Collections.Generic;
namespace Assets.Core.InventoryManagement
{
    class InventoryManager
    {
        List<ItemSlot> itemSlots;

        public InventoryManager()
        {
            itemSlots = new List<ItemSlot>();
        }

        public void AddItem(Item item, int amount)
        {
            int indexOfItem = findItem(item);
            if (indexOfItem != -1)
                itemSlots[indexOfItem].Amount += amount;
            else
                itemSlots.Add(new ItemSlot(item, amount));
        }


        public void AddItem(Item item)
        {
            itemSlots.Add(new ItemSlot(item, 1));
        }
        private int findItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
