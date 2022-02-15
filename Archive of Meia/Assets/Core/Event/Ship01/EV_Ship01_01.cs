using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EV_Ship01_01 : MonoBehaviour
{
    public TextAsset File;

    [SerializeField]
    private string Name;
    [SerializeField]
    private int[] DialoguesIndex;

    public int EventID;
    public GameObject TextCenter;

    private GameObject CanvasCache;
    private GameObject DialogueText;
    private GameObject DialogueName;
    private GameObject DialogueBox;
    private float[] TextRGB = new float[3];

    private string[,] TextTable;

    private void Awake()
    {
        if (!TheGameManager.Events[EventID])
        {
            CanvasCache = GameObject.Find("CanvasCache");
            CanvasCache.GetComponent<IntroScene>().enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!TheGameManager.Events[EventID])
        {
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

            StartCoroutine(scene());
            TheGameManager.Events[EventID] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator scene()
    {
        float i = 0;
        int index = 0;
        GameObject g;

        g = Instantiate(TextCenter, Vector2.zero, new Quaternion());
        g.transform.SetParent(GameObject.Find("Canvas").transform);
        g.transform.localScale = new Vector3(1, 1, 1);
        g.transform.localPosition = Vector2.zero;
        g.name = "TextCenter";
        //g.transform.GetChild(0).GetComponent<Text>().text = "";

        // Phase de texte sur fond noir
        while (index < DialoguesIndex.Length)
        {
            // Verifie la validite du texte
            if (File == null)
            {
                g.GetComponent<Text>().text = "?!PLACEHOLDER?!\nMISSING FILE !!";
                index = DialoguesIndex.Length;
            }
            else if (TextTable.GetLength(1) > DialoguesIndex[index])
            {
                g.GetComponent<Text>().text = TextTable[TheGameManager.lang, DialoguesIndex[index]];
            }
            else
            {
                g.GetComponent<Text>().text = "?!PLACEHOLDER?!\nFILE: " + File.name + "\nDIAL ROW: " + DialoguesIndex[index];
            }

            // Eclaircit le texte
            do
            {
                g.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
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
            g.GetComponent<Text>().color = new Color(TextRGB[0], TextRGB[1], TextRGB[2], i);
            yield return new WaitForSeconds(1 / 50f);
        } while (i > 0);

        CanvasCache.GetComponent<IntroScene>().enabled = true;
    }

}
