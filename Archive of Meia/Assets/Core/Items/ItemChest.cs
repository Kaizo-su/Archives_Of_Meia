using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemChest : Chest
{
    static private bool opened;

    public byte IndexObjet;

    private byte param = 0;

    private void lootItem(byte p)
    {
        param = p;
        GameObject.Find("Player").GetComponent<InventoryCC>().SetItems(p, 1);
    }

    protected override void loot()
    {
        lootItem(IndexObjet);
    }

    protected override string WhatInsideChest()
    {
        return "" + GameObject.Find("Player").GetComponent<InventoryCC>().GetItems(param).Name;
    }
}
