using UnityEngine;

public class MoneyChest : Chest
{
    static private bool opened;

    public int cash;


    protected override void loot()
    {
        isMoneyChest = true;
        GameObject.Find("Player").GetComponent<InventoryCC>().SetMoney(cash);
    }

    protected override string WhatInsideChest()
    {
        return "" + cash;
    }
}
