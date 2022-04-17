using System.Collections;
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
    public bool NeedSpecificWeaponToContinue;
    public Vector3 Arrival;

    private GameObject P;
    private GameObject G;
    //private GameObject L;
    private GameObject C;
    private GameObject D;

    private GameObject Cam;
    
    [SerializeField]
    private TextAsset File;
    [SerializeField]
    private int[] DialoguesIndex;

    private bool onButtonDown;

    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.Find("Player");
        G = GameObject.Find("Character");
        C = GameObject.Find("CanvasCache");
        //L = GameObject.Find("Directional Light");
        Cam = GameObject.Find("Camera");
        D = this.transform.GetChild(0).gameObject;
        onButtonDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            onButtonDown = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            onButtonDown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Teleporte au contact
        if(other.name == P.name && UseButtunToTeleport == false && NeedSpecificWeaponToContinue == false)
        {
            while (orientation > 360)
            {
                orientation -= 360;
            }

            StartCoroutine(Teleportation(0.5f));

        }

        //Affiche la commande d'action si la teleportation s'active via un bouton
        if (UseButtunToTeleport == true)
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.white;
            GameObject.Find("T_Action").GetComponent<Text>().text = "Entrer";
        }

        //Reffuse le passage si le personnage n'est pas équipé de l'objet
        if (NeedSpecificWeaponToContinue == true)
        {
            if (P.GetComponent<PlayerCC>().GetWeapon() == null)
            {

                StartCoroutine(Diaolgue());
            }
            else
            {
                StartCoroutine(Teleportation(0.5f));
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Teleporte via un bouton
        if (onButtonDown && UseButtunToTeleport == true && NeedSpecificWeaponToContinue == false)
        {

            //GameObject.Find("Character").transform.LookAt(this.transform);
            this.GetComponent<Collider>().enabled = false;
            //this.transform.localEulerAngles = new Vector3(100, 0, 0);

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            StartCoroutine(Teleportation(0.5f));
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
            TheGameManager.Dest = Arrival;
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
        this.GetComponent<Collider>().enabled = true;
    }

    protected IEnumerator Diaolgue()
    {
        GameObject DialogueText = GameObject.Find("DialogueText");
        GameObject DialogueName = GameObject.Find("DialogueName");
        GameObject DialogueBox = GameObject.Find("DialogueBox");
        float[] TextRGB = new float[3];
        TextRGB[0] = DialogueText.GetComponent<Text>().color.r;
        TextRGB[1] = DialogueText.GetComponent<Text>().color.g;
        TextRGB[2] = DialogueText.GetComponent<Text>().color.b;
        DialogueText.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], 0);
        DialogueName.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], 0);
        DialogueBox.GetComponent<Image>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], 0);

        string[,] TextTable = TheDialogueManager.TextExtractor(File);


        GameObject.Find("Player").GetComponent<PlayerCC>().Movable = false;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(false);

        this.GetComponent<Collider>().enabled = false;

        //DialogueName.GetComponent<Text>().text = Name;

        float i = 0;
        int index = 0;
        // Eclaircit la boite
        do
        {
            DialogueName.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
            DialogueBox.GetComponent<Image>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
            i += 0.2f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i < 1);

        i = 0;

        while (index < DialoguesIndex.Length)
        {
            if (File == null)
            {
                DialogueText.GetComponent<Text>().text = "?!PLACEHOLDER?!\nMISSING FILE !!";
                index = DialoguesIndex.Length;
            }
            else if (TextTable.GetLength(1) > DialoguesIndex[index])
            {
                DialogueText.GetComponent<Text>().text = TextTable[TheGameManager.lang, DialoguesIndex[index]];
            }
            else
            {
                DialogueText.GetComponent<Text>().text = "?!PLACEHOLDER?!\nFILE: " + File.name + "\nDIAL ROW: " + DialoguesIndex[index];
            }

            // Eclaircit le texte
            do
            {
                DialogueText.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
                i += 0.075f;
                yield return new WaitForSeconds(1 / 50f);
            } while (i < 1);

            i = 0;
            index++;

            do
            {
                yield return null;
            } while (!Input.GetButtonDown("Fire2"));
        }

        i = 1;

        // Efface le texte et la boite
        do
        {
            i -= 0.075f;
            DialogueText.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
            DialogueName.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
            DialogueBox.GetComponent<Image>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
            yield return new WaitForSeconds(1 / 50f);
        } while (i > 0);

        GameObject.Find("Player").GetComponent<PlayerCC>().Movable = true;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(true);

        this.GetComponent<Collider>().enabled = true;
        //onDialogue = false;
    }
}
