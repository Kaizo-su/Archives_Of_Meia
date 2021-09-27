using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    private bool opened;
    private bool canOpen;

    // Start is called before the first frame update
    void Start()
    {
        canOpen=false;
        opened=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && !opened && canOpen==true)
        {
            opened = true;

            GameObject.Find("Character").transform.LookAt(this.transform);
            this.GetComponent<Collider>().enabled = false;

            this.transform.GetChild(0).localEulerAngles = new Vector3(0, 25, 0);
            this.transform.parent.GetChild(1).position = this.transform.parent.GetChild(1).position - new Vector3(0, 3, 0);


            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!opened)
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.white;
            GameObject.Find("T_Action").GetComponent<Text>().text = "Activer";
            canOpen=true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
        GameObject.Find("T_Action").GetComponent<Text>().text = "";
        canOpen=false;
    }

}
