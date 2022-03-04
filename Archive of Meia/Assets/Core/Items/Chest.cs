using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public abstract class Chest : MonoBehaviour
{

    [SerializeField]
    private TextAsset File;
    [SerializeField]
    private int[] DialoguesIndex;

    private bool opened;
    private bool canOpen;

    protected bool isMoneyChest;

    private Transform couvercle;

    private GameObject DialogueText;
    private GameObject DialogueName;
    private GameObject DialogueBox;
    private float[] TextRGB = new float[3];

    private string[,] TextTable;

    // Start is called before the first frame update
    void Start()
    {
        couvercle = this.transform.GetChild(0);

        if (opened)
        {
            this.GetComponent<Collider>().enabled = false;
            this.transform.localEulerAngles = new Vector3(100, 0, 0);
        }
        canOpen=false;

        DialogueText = GameObject.Find("DialogueText");
        DialogueName = GameObject.Find("DialogueName");
        DialogueBox = GameObject.Find("DialogueBox");
        TextRGB[0] = DialogueText.GetComponent<Text>().color.r;
        TextRGB[1] = DialogueText.GetComponent<Text>().color.g;
        TextRGB[2] = DialogueText.GetComponent<Text>().color.b;
        DialogueText.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], 0);
        DialogueName.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], 0);
        DialogueBox.GetComponent<Image>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], 0);

        TextTable = TheDialogueManager.TextExtractor(File);
    }

    protected abstract void loot();

    protected abstract string WhatInsideChest();

    void Update()
    {

        if (Input.GetButtonDown("Fire2") && canOpen == true && opened == false)
        {
            opened = true;

            //GameObject.Find("Character").transform.LookAt(this.transform);
            //this.transform.GetChild(0).GetComponent<Collider>().enabled = false;
            couvercle.transform.localEulerAngles = new Vector3(100, 0, 0);

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            StartCoroutine(Opening());
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.white;
            GameObject.Find("T_Action").GetComponent<Text>().text = "Ouvrir";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            canOpen = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            canOpen = true;
        }
    }


    protected IEnumerator Opening()
    {
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=false;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(false);

        float i = 0;
        int index = 0;

        do
        {
            couvercle.localEulerAngles = new Vector3(i, 0, 0);
            i += 2f;
            yield return new WaitForEndOfFrame();

        } while (couvercle.localEulerAngles.x <= 80);

        loot();

        //yield return new WaitForSeconds(1);

        this.GetComponent<Collider>().enabled = false;

        DialogueName.GetComponent<Text>().text = "";

        i = 0;
        // Eclaircit la boite
        do
        {
            DialogueName.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
            DialogueBox.GetComponent<Image>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
            i += 0.2f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i < 1);

        i = 0;

        if (File == null)
        {
            DialogueText.GetComponent<Text>().text = "?!PLACEHOLDER?!\nMISSING FILE !!";
            index = DialoguesIndex.Length;
        }
        else if (TextTable.GetLength(1) > DialoguesIndex[index])
        {
            if (isMoneyChest)
            {
                DialogueText.GetComponent<Text>().text = TextTable[TheGameManager.lang, DialoguesIndex[index]] + " " + WhatInsideChest() + " " +TextTable[TheGameManager.lang, DialoguesIndex[index + 1]] + ".";
            }
            else
            {
                DialogueText.GetComponent<Text>().text = TextTable[TheGameManager.lang, DialoguesIndex[index]] + "\n" + WhatInsideChest();
            }
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

        if (!isMoneyChest)
        {
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
    }
}
