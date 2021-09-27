﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string Destination;
    public float orientation;

    private GameObject P;
    private GameObject G;

    private GameObject C;
    private GameObject D;

    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.Find("Player");
        G = GameObject.Find("Character");
        C = GameObject.Find("CanvasCache");
        D = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == P.name)
        {
            while (orientation > 360)
            {
                orientation -= 360;
            }

            StartCoroutine(Teleportation(0.5f));
            
        }
    }

    IEnumerator Teleportation(float p)
    {
        P.GetComponent<PlayerCC>().Movable=false;
        G.GetComponent<PlayerOrientation>().SetOrientable(false);
        /*
        yield return new WaitForSeconds(p / 3);

        C.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        */

        float i = 0;

        do
        {
            C.GetComponent<Image>().color = new Color(0, 0, 0, i);
            i += 0.075f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i < 1);



        if (Destination == "")
        {
            P.transform.position = new Vector3(D.transform.position.x, D.transform.position.y +1 , D.transform.position.z);
            CameraCC.Teleport= true;
        }
        else
        {
            SceneManager.LoadScene(Destination);
        }

        if (orientation >= 0)
        {
            G.transform.rotation = Quaternion.Euler(G.transform.rotation.x, orientation, G.transform.rotation.z);
        }
        
        yield return new WaitForSeconds(p);

        i = 1;

        do
        {
            C.GetComponent<Image>().color = new Color(0, 0, 0, i);
            i -= 0.075f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i > 0);


        yield return new WaitForSeconds(p / 3);
        C.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

        yield return new WaitForSeconds(p / 3);
        P.GetComponent<PlayerCC>().Movable=true;
        G.GetComponent<PlayerOrientation>().SetOrientable(true);
    }
}