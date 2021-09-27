using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_GoldKey : MonoBehaviour
{
    private Image I_GoldKey;
    private bool key;

    // Start is called before the first frame update
    void Awake()
    {
        I_GoldKey = GameObject.Find("I_GoldKey").GetComponent<Image>();
    }

    public bool Key { 
        get => key;
        set
        {
            key = value;
            I_GoldKey.color = key ? Color.white : Color.clear;

        }
    }
}
