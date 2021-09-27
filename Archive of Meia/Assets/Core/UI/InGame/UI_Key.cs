using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Key : MonoBehaviour
{
    private Image I_Key;
    private int nbKeys;
    public int Keys {
        get => nbKeys;
        set
        {
            nbKeys = value;
            I_Key.color = nbKeys > 0 ? Color.white : Color.clear;
            GetComponent<Text>().text = nbKeys > 0 ? nbKeys.ToString() : "";
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        I_Key = GameObject.Find("I_Key").GetComponent<Image>();
    }
}
