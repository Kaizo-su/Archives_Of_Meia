using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "PNJ_Default", menuName = "PNJ settings")]
public class PNJ : MonoBehaviour
{
    [SerializeField]
    private TextAsset File;
    [SerializeField]
    private string Name;
    [SerializeField]
    private int[] DialoguesIndex;

    private bool onDialogue;
    private bool onButtonDown;

    private GameObject DialogueText;
    private GameObject DialogueName;
    private GameObject DialogueBox;
    private float[] TextRGB = new float[3];

    private string[,] TextTable;


    void Start()
    {
        onDialogue = false;
        onButtonDown = false;

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
    void Update()

    {
        if (Input.GetButtonDown("Fire2"))
        {
            onButtonDown = true;
        }
        else if(Input.GetButtonUp("Fire2"))
        {
            onButtonDown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.white;
            GameObject.Find("T_Action").GetComponent<Text>().text = "Parler";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            if (onButtonDown && !onDialogue)
            {
                onDialogue = true;

                GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
                GameObject.Find("T_Action").GetComponent<Text>().text = "";
                StartCoroutine(Diaolgue());
            }
        }
    }

    protected IEnumerator Diaolgue()
    {
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable = false;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(false);

        this.GetComponent<Collider>().enabled = false;

        DialogueName.GetComponent<Text>().text = Name;

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
            else if(TextTable.GetLength(1) > DialoguesIndex[index])
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
        onDialogue = false;
    }
}
