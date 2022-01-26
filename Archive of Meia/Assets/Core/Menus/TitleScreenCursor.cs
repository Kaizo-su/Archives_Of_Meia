using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenCursor : MonoBehaviour
{
    private int option;
    private int cooldown;
    private int waiting;

    private string[] stringTable;
    private string[,] TextTable;
    private string textFile;
    public TextAsset File;

    private GameObject NewGame;
    private GameObject Continue;
    private GameObject Option;
    private GameObject Quit;

    private bool locked;

    private Image I;

    int menuItemHeight = 80;

    private string[][] coroutine = new string[][]
    {
        //                  Action   | Params (optionnels)
      /*0*/  new string[]{"LoadScene", "Ship01"},
      /*1*/  new string[]{""},
      /*2*/  new string[]{""},
      /*3*/  new string[]{"Quit"},
    };

    // Start is called before the first frame update
    void Start()
    {
        NewGame = GameObject.Find("NewGame");
        Continue = GameObject.Find("Continue");
        Option = GameObject.Find("Option");
        Quit = GameObject.Find("Quit");

        I = GameObject.Find("CanvasCache").GetComponent<Image>();
        option = 0;
        cooldown = 45;
        waiting = cooldown;


        //Recupere le texte du fichier
        if (File == null)
        {
            textFile = "NO FILE";
        }
        else
        {
            textFile = File.ToString();
        }

        int x = 0;
        int y = 0;
        int l = 0;

        //Repaire toute les entrees du texte
        stringTable = textFile.Split(';', '\n');

        //Calcule le nombre de colone et de ligne
        for (int i = 0; i < File.ToString().Length; i++)
        {
            if (textFile[i] == '\n')
            {
                y++;
            }
            if (textFile[i] == ';')
            {
                x++;
            }
        }

        //Declare un tableau selon les ligne et les colones
        TextTable = new string[(x / y) + 1, y];

        //Remplis le tableau avec les entrees de textes
        for (int j = 0; j < y; j++)
        {
            for (int k = 0; k < (x / y) + 1; k++)
            {
                TextTable[k, j] = stringTable[l];
                l++;
            }
        }

        NewGame.GetComponent<Text>().text = TextTable[TheGameManager.lang, 1];
        Continue.GetComponent<Text>().text = TextTable[TheGameManager.lang, 2];
        Option.GetComponent<Text>().text = TextTable[TheGameManager.lang, 3];
        Quit.GetComponent<Text>().text = TextTable[TheGameManager.lang, 4];
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") >= 0.5f && waiting == cooldown))
            {
                option--;
                waiting = 0;
            }
            else if (Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("Vertical") <= -0.5f && waiting == cooldown))
            {
                option++;
                waiting = 0;
            }

            option += coroutine.Length;
            option %= coroutine.Length;

            transform.localPosition = new Vector3(transform.localPosition.x, -menuItemHeight * option, transform.localPosition.z);

            if (Input.GetButtonDown("Fire2"))
            {
                locked = true;
                StartCoroutine("RunMenuAction", coroutine[option]);
            }
        }

        if (waiting < cooldown)
        {
            waiting++;
        }
    }

    IEnumerator RunMenuAction(object value)
    {
        string[] menuOption = (string[])value;
        float i = 0;
        if (menuOption[0] == "")
        {
            locked = false;
            yield break;
        }
        else
        {
            do
            {
                I.color = new Color(0, 0, 0, i);
                i += 0.02f;
                yield return new WaitForSeconds(1 / 50f);
            } while (i < 1);

            I.color = new Color(0, 0, 0, 1);

            yield return new WaitForSeconds(2f);
            switch (menuOption[0])
            {
                case "LoadScene":
                    SceneManager.LoadScene(menuOption[1].ToString());
                    break;

                case "quit":
                    Application.Quit();
                    break;
            }
        }

    }
}
