using UnityEngine;

public class KeyChest : Chest
{
    static private bool opened;

    private void lootKey()
    {
        GameObject.Find("Player").GetComponent<InventoryCC>().AddKeys(1);
    }

    protected override void loot()
    {
        lootKey();
    }
}
