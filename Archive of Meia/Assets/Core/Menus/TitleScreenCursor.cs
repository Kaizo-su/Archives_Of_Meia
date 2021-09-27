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

    private bool locked;

    private Image I;

    int menuItemHeight = 80;
    private string[][] coroutine = new string[][]
    {
        //                  Action   | Params (optionnels)
      /*0*/  new string[]{"LoadScene", "DJ0"},
      /*1*/  new string[]{""},
      /*2*/  new string[]{""},
      /*3*/  new string[]{"Quit"},
    };

    // Start is called before the first frame update
    void Start()
    {
        I = GameObject.Find("CanvasCache").GetComponent<Image>();
        option = 0;
        cooldown = 20;
        waiting = cooldown;
    }
    private void FixedUpdate()
    {
        waiting++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") >= 0.5f && waiting == cooldown))  // Le vertical fait bugger les fleches
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

            if (Input.GetButtonDown("Fire1"))
            {
                locked = true;
                StartCoroutine("RunMenuAction", coroutine[option]);
            }
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
