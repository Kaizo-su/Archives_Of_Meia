using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Equipment: MonoBehaviour
{
    private int option;
    private int optInv;
    private int cooldown;
    private int waiting;
    private int ItemMax;
    private int KeyItemMax;
    private int hiddenObject;

    private float inerTime;
    private float posX;
    private float posY;

    public GameObject UI_Item;

    private PlayerCC PCC;

    // Variables des éléments de UI du tableau d'Inventaire
    private Image ItemImage;
    private Text Description;
    private Transform Cursor;

    // Liste des objets d'inventaire
    private List<Weapon> Weapons;
    private List<Protecter> Protecters;

    // Start is called before the first frame update
    void Start()
    {
        option = 0;             // Choix dans l'inventaire  (position X)
        optInv = 0;             // Choix d'inventaire       (position Y)
        ItemMax = 8;            // Maximum d'objet dans l'inventaire de base
        KeyItemMax = 12;        // Maximum d'objet dans l'inventaire Clé

        hiddenObject = 0;

        cooldown = 20;
        waiting = cooldown;
        inerTime = 0;

        // Récupere les élément de UI du tableau d'Inventaire
        ItemImage = transform.GetChild(8).GetComponent<Image>();
        Description = transform.GetChild(7).GetComponent<Text>();
        Cursor = transform.GetChild(9).transform;

        // Récupere la position du curseur
        posX = Cursor.localPosition.x;
        posY = Cursor.localPosition.y;

        Weapons = GameObject.Find("Player").GetComponent<InventoryCC>().GetWeapons();
        Protecters = GameObject.Find("Player").GetComponent<InventoryCC>().GetProtecters();
        PCC = GameObject.Find("Player").GetComponent<PlayerCC>();

        DisplayInventory();
        DisplayKeyInventory();
    }

    // Update is called once per frame
    void Update()
    {
        // Activer/Desactiver la pause
        if (Input.GetButtonDown("Jump"))
        {
            DestroyInventory();
            PCC.TogglePauseGame();
        }

        if (Input.GetButtonDown("Fire1") && UI_Pause.option == 1)
        {
            //TODO trouver l'objet, agir en fonction du type  

        }
        if (Input.GetButtonDown("Fire2"))
        {
            DestroyInventory();
            DisplayInventory();
            DisplayKeyInventory();

        }

        // Déplacement du curseur dans l'inventaire des objets consomables
        if (Input.GetKeyDown(KeyCode.D) && UI_Pause.option == 3 || (Input.GetAxis("Horizontal") >= 0.5f && waiting == cooldown && UI_Pause.option == 1))
        {
            InventoryManager(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A) && UI_Pause.option == 3 || (Input.GetAxis("Horizontal") <= -0.5f && waiting == cooldown && UI_Pause.option == 1))
        {
            InventoryManager(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.W) && UI_Pause.option == 3 || (Input.GetAxis("Vertical") >= 0.5f && waiting == cooldown && UI_Pause.option == 1))
        {
            InventoryManager(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) && UI_Pause.option == 3 || (Input.GetAxis("Vertical") <= -0.5f && waiting == cooldown && UI_Pause.option == 1))
        {
            InventoryManager(0, -1);
        }

        if (waiting < cooldown){
            waiting++;
        }

        // Change la couleur du selecteur
        Cursor.GetComponent<Image>().color = new Color(255, 255, 255, (Mathf.Sin(inerTime * 25) / 3) + 0.66f);
        inerTime += 0.001f;
        if (inerTime >= Mathf.PI)
            inerTime -= Mathf.PI;
    }

    public void DisplayInventory()
    {
        GameObject g;

        int nbObjets = (Weapons.Count < ItemMax ? Weapons.Count : ItemMax);
        
        for (int i = 0; i < nbObjets; i++)
        {
                g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
                g.transform.SetParent(this.transform);
                g.transform.localScale = new Vector3(1, 1, 1);
                g.transform.localPosition = new Vector3(posX + (i * 64), posY);
                g.name = "Item" + i;
                g.GetComponent<Image>().sprite = Weapons[i].Sprite;
                //g.transform.GetChild(0).GetComponent<Text>().text = Items[i].Qt.ToString();
        }
        
    }

    public void DisplayKeyInventory()
    {
        GameObject g;

        int nbObjets = (Protecters.Count < KeyItemMax ? Protecters.Count : KeyItemMax);

        for (int i = 0; i < nbObjets; i++)
        {
            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector3(posX + (i * 64), posY - 75);
            g.name = "KeyItem" + i + hiddenObject;
            g.GetComponent<Image>().sprite = Protecters[i].Sprite;
            //g.transform.GetChild(0).GetComponent<Text>().text = KeyItems[i].Qt.ToString();
        }

    }

    private void DescriptionInventory(int i)
    {

        Description.text = Weapons[i].Name + "\n\n" + Weapons[i].Description;
        ItemImage.sprite = Weapons[i].Sprite;
    }
    
    private void DescriptionKeyInventory(int i)
    {
        
        Description.text = Protecters[i].Name + "\n\n" + Protecters[i].Description;
        ItemImage.sprite = Protecters[i].Sprite;
    }

    private void DestroyInventory()
    {
        GameObject[] UI = GameObject.FindGameObjectsWithTag("UI_Item");

        for (int i = 0; i < UI.Length; i++)
        {
            Destroy(UI[i]);
        }
    }

    private void InventoryManager(int x, int y)
    {
        option += x;
        optInv += y;
        
        waiting = 0;
        
        // Si on sort des limites de l'inventaire par en haut ou par en bas
        if (optInv > 1)
            optInv = 0;
        if (optInv <0)
            optInv = 1;

        // Si dans l'inventaire de base
        if (optInv == 0)
        {
            // Si on sort des limites de l'inventaire par en les cotés
            if (option > ItemMax - 1)
                option = 0;
            if (option < 0)
                option = ItemMax - 1;
            DescriptionInventory(option);
        }
        // Si dans l'inventaire Clé
        else
        {
            // Si on sort des limites de l'inventaire par en les cotés
            if (option > KeyItemMax - 1)
                option = 0;
            if (option < 0)
                option = KeyItemMax - 1;
            DescriptionKeyInventory(option);
        }

        //Positionne le curseur sur un objet selon les variables d'option
        Cursor.localPosition = new Vector2(posX + (option * 64), posY - (optInv == 0 ? 0 : 64));
    }
}
