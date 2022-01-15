using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string Destination;
    public float orientation;   // 0 = Nord --- 90 = Est --- 180 = Sud --- 270 = Ouest
    public byte typeView;       // 0 = Pas de changement --- 1 = WorldMap --- 2 = Interior --- 3 = Exterior
    //public byte light;          // 0 = Off --- 1 = On --- Pas de changemant
    public bool UseButtunToTeleport;

    private GameObject P;
    private GameObject G;
    //private GameObject L;
    private GameObject C;
    private GameObject D;

    private GameObject Cam;

    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.Find("Player");
        G = GameObject.Find("Character");
        C = GameObject.Find("CanvasCache");
        //L = GameObject.Find("Directional Light");
        Cam = GameObject.Find("Camera");
        D = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && UseButtunToTeleport == true)
        {

            GameObject.Find("Character").transform.LookAt(this.transform);
            this.GetComponent<Collider>().enabled = false;
            this.transform.localEulerAngles = new Vector3(100, 0, 0);

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            StartCoroutine(Teleportation(0.5f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == P.name && UseButtunToTeleport == false)
        {
            while (orientation > 360)
            {
                orientation -= 360;
            }

            StartCoroutine(Teleportation(0.5f));
            
        }

        if(UseButtunToTeleport == true)
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.white;
            GameObject.Find("T_Action").GetComponent<Text>().text = "Entrer";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
        GameObject.Find("T_Action").GetComponent<Text>().text = "";
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

        // ###########################
        // Assombrit l ecran
        // ###########################
        do
        {
            C.GetComponent<Image>().color = new Color(0, 0, 0, i);
            i += 0.075f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i < 1);

        // ###########################
        // Teleporte
        // ###########################
        if (Destination == "")
        {
            Cam.GetComponent<CameraCC>().SetTeleport(true);
            P.transform.position = new Vector3(D.transform.position.x, D.transform.position.y +1 , D.transform.position.z);
            Cam.GetComponent<CameraCC>().SetTeleport(false);
            Cam.GetComponent<CameraCC>().ResetCameraPosition();
        }
        else
        {
            SceneManager.LoadScene(Destination);
        }

        if (orientation >= 0)
        {
            G.transform.rotation = Quaternion.Euler(G.transform.rotation.x, orientation, G.transform.rotation.z);
        }

        if(Cam != null)
        {
            Cam.GetComponent<CameraCC>().changeViewTypeTo(typeView);
            //Cam.GetComponent<CameraCC>().ResetCamPosition();
        }

        yield return new WaitForSeconds(p);

        i = 1;

        // ###########################
        // Eclaircit l'ecran
        // ###########################
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
