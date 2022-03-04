using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EquipChest : Chest
{
    static private bool opened;

    public byte IndexObjet;
    public byte TypeObjet;

    private byte param = 0;

    private void lootItem(byte p)
    {
        param = p;

        switch (TypeObjet)
        {
            case 0:
                GameObject.Find("Player").GetComponent<InventoryCC>().SetWeapons(p, 1);
                break;
            case 1:
                GameObject.Find("Player").GetComponent<InventoryCC>().SetProtecters(p, 1);
                break;
            case 2:
                GameObject.Find("Player").GetComponent<InventoryCC>().SetNecklaces(p, 1);
                break;
            case 3:
                GameObject.Find("Player").GetComponent<InventoryCC>().SetAccessories(p, 1);
                break;
        }
        
    }

    protected override void loot()
    {
        lootItem(IndexObjet);
    }

    protected override string WhatInsideChest()
    {
        switch (TypeObjet)
        {
            case 0:
                return "" + GameObject.Find("Player").GetComponent<InventoryCC>().GetWeapon(param).Name;
            case 1:
                return "" + GameObject.Find("Player").GetComponent<InventoryCC>().GetProtecter(param);
            case 2:
                return "" + GameObject.Find("Player").GetComponent<InventoryCC>().GetNecklace(param);
            case 3:
                return "" + GameObject.Find("Player").GetComponent<InventoryCC>().GetAccessory(param);
            default:
                return "";
        }
    }
}
