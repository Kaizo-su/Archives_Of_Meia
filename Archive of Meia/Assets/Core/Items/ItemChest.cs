using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemChest : Chest
{
    static private bool opened;

    public int IndexObjet;

    private void lootItem(int p)
    {
        GameObject.Find("Player").GetComponent<InventoryCC>().AddObject(p);
    }

    protected override void loot()
    {
        lootItem(IndexObjet);
    }
}
