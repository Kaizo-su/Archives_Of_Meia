using UnityEngine;

public class KeyChest : Chest
{
    static private bool opened;

    private void lootKeyItem()
    {
        GameObject.Find("Player").GetComponent<InventoryCC>().SetKeyItems(0, 1);
    }

    protected override void loot()
    {
        lootKeyItem();
    }

    protected override string WhatInsideChest()
    {
        return "" + GameObject.Find("Player").GetComponent<InventoryCC>().GetKeyItem(0).Name;
    }
}
