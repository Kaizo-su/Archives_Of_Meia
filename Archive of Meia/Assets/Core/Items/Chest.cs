﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class Chest : MonoBehaviour
{
    private bool opened;
    private bool canOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        if (opened)
        {
            this.GetComponent<Collider>().enabled = false;
            this.transform.localEulerAngles = new Vector3(100, 0, 0);
        }
        canOpen=false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("I_Action").GetComponent<Image>().color = Color.white;
        GameObject.Find("T_Action").GetComponent<Text>().text = "Ouvrir";
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
        GameObject.Find("T_Action").GetComponent<Text>().text = "";
        canOpen=false;
    }

    protected abstract void loot();

    void Update () {

        if (Input.GetButtonDown("Fire2") && canOpen==true && opened==false)
        {
            opened = true;

            GameObject.Find("Character").transform.LookAt(this.transform);
            this.GetComponent<Collider>().enabled = false;
            this.transform.localEulerAngles = new Vector3(100, 0, 0);

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            StartCoroutine(Opening());
        }

    }

    private void OnTriggerStay(Collider other)
    {
       canOpen=true;  
    }


    protected IEnumerator Opening()
    {
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=false;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(false);

        float i = 0;

        do
        {
            this.transform.localEulerAngles = new Vector3(i, 0, 0);
            i += 2f;
            yield return new WaitForEndOfFrame();

        } while (this.transform.localEulerAngles.x <= 80);

        loot();

        //yield return new WaitForSeconds(1);

        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=true;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(true);

    }
}