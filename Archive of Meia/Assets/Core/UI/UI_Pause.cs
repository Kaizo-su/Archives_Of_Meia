using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    public static int option = 2;

    private Transform[] Panels;


    // Start is called before the first frame update
    void Start()
    {
        Panels = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            Panels[i] = transform.GetChild(i).transform;
        ActualisationPanels();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("R"))
            SlidePanel(1);
        if (Input.GetButtonDown("L"))
            SlidePanel(-1);
    }

    private void SlidePanel(int p)
    {
        option += Panels.Length + p;
        option %= Panels.Length;

        for(int i = Panels.Length - 1; i >= 0; i--)
            Panels[i].localPosition = new Vector2((i - option) * 1000 ,Panels[i].localPosition.y);
    }

    // Fonction qui met à jour tout les menus de l'interface de pause
    public void ActualisationPanels()
    {
        if (Panels == null)
            return;

        Panels[2].GetComponent<UI_Stat>().Actualisation();

        //TO DO Afficher description d'items

        /*
        Panels[3].GetComponent<UI_Inventory>().DestroyInventory();
        Panels[3].GetComponent<UI_Inventory>().DisplayInventory();

        Panels[1].GetComponent<UI_Equipment>().DestroyInventory();
        Panels[1].GetComponent<UI_Equipment>().DisplayWeapons();*/
    }
}
