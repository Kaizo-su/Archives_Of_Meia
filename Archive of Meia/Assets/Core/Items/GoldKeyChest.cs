using UnityEngine.UI;
using UnityEngine;

public class GoldKeyChest : Chest
{
    static private bool opened;

    static private bool openable;

    private void lootGoldKey()
    {
        GameObject.Find("Player").GetComponent<InventoryCC>().SetKeyItems(1, 1);
    }

    protected override void loot()
    {
        lootGoldKey();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            openable = other.GetComponent<InventoryCC>().GetKeyItem(0).Qt != 0;
            GameObject.Find(openable ? "I_Action" : "I_NAction").GetComponent<Image>().color = Color.white;
        }
        GameObject.Find("T_Action").GetComponent<Text>().text = "Ouvrir";
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
        GameObject.Find("I_NAction").GetComponent<Image>().color = Color.clear;
        GameObject.Find("T_Action").GetComponent<Text>().text = "";
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetButtonDown("Fire2") && openable && !opened)
        {
            opened = true;

            //GameObject.Find("Character").transform.LookAt(transform);
            GetComponent<Collider>().enabled = false;
            //transform.localEulerAngles = new Vector3(100, 0, 0);

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            GameObject.Find("Player").GetComponent<InventoryCC>().SetKeyItems(0, -1);
            StartCoroutine(Opening());
        }
    }

    protected override string WhatInsideChest()
    {
        return "" + GameObject.Find("Player").GetComponent<InventoryCC>().GetKeyItem(1).Name;
    }
}
