using UnityEngine.UI;
using UnityEngine;

public class GoldKeyChest : Chest
{
    static private bool opened;

    static private bool openable;

    // Start is called before the first frame update
    void Start()
    {
        if (opened)
        {
            GetComponent<Collider>().enabled = false;
            transform.localEulerAngles = new Vector3(100, 0, 0);
        }
    }

    private void lootGoldKey()
    {
        //TO DO
        //GameObject.Find("Player").GetComponent<InventoryCC>().GoldKey=true;
    }

    protected override void loot()
    {
        lootGoldKey();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // TO DO
            //openable = other.GetComponent<InventoryCC>().Key != 0;
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

            GameObject.Find("Character").transform.LookAt(transform);
            GetComponent<Collider>().enabled = false;
            transform.localEulerAngles = new Vector3(100, 0, 0);

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            //GameObject.Find("Player").GetComponent<InventoryCC>().AddKeys(-1);
            GameObject.Find("Player").GetComponent<InventoryCC>().SetKeyItems(0, -1);
            StartCoroutine(Opening());
        }
    }
}
