using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LangSelector : MonoBehaviour
{
    private Image MonImage;
    private RectTransform MaPosition;

    private int flag;
    private int cooldown;
    private int waiting;

    // Start is called before the first frame update
    void Start()
    {
        flag = 0;
        cooldown = 45;
        waiting = cooldown;
        MonImage = this.GetComponent<Image>();
        MaPosition = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Selectionne une langue avec les flèches
        if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") >= 0.5f && waiting == cooldown))
        {
            flag -= 3;
            waiting = 0;
        }
        else if (Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("Vertical") <= -0.5f && waiting == cooldown))
        {
            flag += 3;
            waiting = 0;
        }
        else if (Input.GetKeyDown(KeyCode.A) || (Input.GetAxis("Horizontal") <= -0.5f && waiting == cooldown))
        {
            flag--;
            waiting = 0;
        }
        else if (Input.GetKeyDown(KeyCode.D) || (Input.GetAxis("Horizontal") >= 0.5f && waiting == cooldown))
        {
            flag++;
            waiting = 0;
        }

        if (flag > 5)
            flag -= 6;
        else if (flag < 0)
            flag += 6;

        switch (flag)
        {
            case 0:
                MaPosition.localPosition = new Vector2(-315, 125);
                break;

            case 1:
                MaPosition.localPosition = new Vector2(0, 125);
                break;

            case 2:
                MaPosition.localPosition = new Vector2(315, 125);
                break;


            case 3:
                MaPosition.localPosition = new Vector2(-315, -165);
                break;

            case 4:
                MaPosition.localPosition = new Vector2(0, -165);
                break;

            case 5:
                MaPosition.localPosition = new Vector2(315, -165);
                break;

        }

        if (waiting < cooldown)
        {
            waiting++;
        }

        //Selectionne la langue
        if (Input.GetButtonDown("Fire2"))
        {
            switch (flag)
            {
                case 0:
                    TheGameManager.lang = 2; //EN
                    break;

                case 1:
                    TheGameManager.lang = 1; //FR
                    break;

                case 2:
                    TheGameManager.lang = 3; //NL
                    break;


                case 3:
                    TheGameManager.lang = 4; //ES
                    break;

                case 4:
                    TheGameManager.lang = 2; // N/A -> EN
                    break;

                case 5:
                    TheGameManager.lang = 5; //EP
                    break;

            }
            SceneManager.LoadScene(1);
        }


        //Change la couleur du selecteur
        MonImage.color = new Color(255, 255, 255, (Mathf.Sin(Time.time * 5)/3)+0.66f);
    }
}
