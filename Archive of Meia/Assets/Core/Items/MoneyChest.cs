using UnityEngine;

public class MoneyChest : Chest
{
    static private bool opened;

    public int cash;

    /*private void AddCash(int p)
    {
        //GameObject.Find("Player").GetComponent<InventoryCC>().SetMoney(p);
    }*/

    protected override void loot()
    {
        GameObject.Find("Player").GetComponent<InventoryCC>().SetMoney(cash);
    }
}
