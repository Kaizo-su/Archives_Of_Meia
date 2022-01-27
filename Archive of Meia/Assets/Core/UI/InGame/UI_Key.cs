using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Key : MonoBehaviour
{
    private Image I_Key;
    public void Keys () {
        {
            int Qt = GameObject.Find("Player").GetComponent<InventoryCC>().GetKeyItem(0).Qt;

            I_Key.color = Qt > 0 ? Color.white : Color.clear;
            GetComponent<Text>().text = Qt > 0 ? Qt.ToString() : "";
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        I_Key = GameObject.Find("I_Key").GetComponent<Image>();
    }
}
