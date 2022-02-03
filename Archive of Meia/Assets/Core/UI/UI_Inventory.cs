using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private int option;
    private int optInv;
    private int cooldown;
    private int waiting;
    private int ItemMax;

    private float inerTime;
    private float posX;
    private float posY;

    private Sprite NoImage;

    public GameObject UI_Item;

    private PlayerCC PCC;

    // Variables des éléments de UI du tableau d'Inventaire
    private Image ItemImage;
    private Text Description;
    private Transform Cursor;

    // Liste des objets d'inventaire
    private List<Item> Items;
    private List<Item> KeyItems;

    // Liste des objets d'inventaire en possession
    private List<Item> MyItems;
    private List<Item> MyKeyItems;

    // Start is called before the first frame update
    void Start()
    {
        option = 0;             // Choix dans l'inventaire  (position X)
        optInv = 0;             // Choix d'inventaire       (position Y)
        ItemMax = 12;           // Maximum d'objet par ligne

        cooldown = 20;
        waiting = cooldown;
        inerTime = 0;

        // Récupere les élément de UI du tableau d'Inventaire
        ItemImage = transform.GetChild(5).GetComponent<Image>();
        Description = transform.GetChild(8).GetComponent<Text>();
        Cursor = transform.GetChild(9).transform;

        NoImage = ItemImage.GetComponent<Image>().sprite;
        Description.GetComponent<Text>().text = "";

        // Récupere la position du curseur
        posX = Cursor.localPosition.x;
        posY = Cursor.localPosition.y;

        Items = GameObject.Find("Player").GetComponent<InventoryCC>().GetItems();
        KeyItems = GameObject.Find("Player").GetComponent<InventoryCC>().GetKeyItems();
        MyItems = new List<Item>();
        MyKeyItems = new List<Item>();

        PCC = GameObject.Find("Player").GetComponent<PlayerCC>();

        DisplayInventory();
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

        if (Input.GetButtonDown("Fire1") && UI_Pause.option == 3)
        {
            //TODO trouver l'objet, agir en fonction du type  

        }
        if (Input.GetButtonDown("Fire2") && UI_Pause.option == 3)
        {
            DestroyInventory();
            DisplayInventory();

        }

        // Déplacement du curseur dans l'inventaire des objets consomables
        if ((Input.GetKeyDown(KeyCode.D) || (Input.GetAxis("Horizontal") >= 0.5f ) && waiting == cooldown && UI_Pause.option == 3))
        {
            InventoryManager(1, 0);
        }
        else if ((Input.GetKeyDown(KeyCode.A) || (Input.GetAxis("Horizontal") <= -0.5f ) && waiting == cooldown && UI_Pause.option == 3))
        {
            InventoryManager(-1, 0);
        }
        else if ((Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") >= 0.5f ) && waiting == cooldown && UI_Pause.option == 3))
        {
            InventoryManager(0, -1);
        }
        else if ((Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("Vertical") <= -0.5f ) && waiting == cooldown && UI_Pause.option == 3))
        {
            InventoryManager(0, 1);
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
        if (Items == null || KeyItems == null)
            return;

        foreach (Item i in Items)
        {
            if (i.Qt > 0)
            {
                MyItems.Add(i);
            }
        }

        foreach (Item i in KeyItems)
        {
            if (i.Qt > 0)
            {
                MyKeyItems.Add(i);
            }
        }

        if (MyItems == null || MyKeyItems == null)
            return;

        int nbObjets;
        GameObject g;

        // Affiche l'inventaire d'objets de base
        nbObjets = (MyItems.Count < ItemMax ? MyItems.Count : ItemMax);
        for (int i = 0; i < nbObjets; i++)
        {
            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector3(posX + (i * 64), posY);
            g.name = "Item" + i;
            g.GetComponent<Image>().sprite = MyItems[i].Sprite;
            g.transform.GetChild(0).GetComponent<Text>().text = MyItems[i].Qt.ToString();
        }

        // Affiche l'inventaire d'objets clés
        nbObjets = (MyKeyItems.Count < ItemMax ? MyKeyItems.Count : ItemMax);
        for (int i = 0; i < nbObjets; i++)
        {
            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector3(posX + (i * 64), posY - 75);
            g.name = "KeyItem" + i;
            g.GetComponent<Image>().sprite = MyKeyItems[i].Sprite;
            g.transform.GetChild(0).GetComponent<Text>().text = MyKeyItems[i].Qt.ToString();
        }
    }

    private void DescriptionInventory(int i)
    {
        if (i == -1 || MyItems == null || MyItems.Count == 0 || MyItems.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            return;
        }
            Description.text = MyItems[i].Name + "\n\n" + MyItems[i].Description;
            ItemImage.sprite = MyItems[i].Sprite;
    }
    
    private void DescriptionKeyInventory(int i)
    {
        if (i == -1 || MyKeyItems == null || MyKeyItems.Count == 0 || MyKeyItems.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            return;
        }

        Description.text = MyKeyItems[i].Name + "\n\n" + MyKeyItems[i].Description;
            ItemImage.sprite = MyKeyItems[i].Sprite;
    }

    public void DestroyInventory()
    {
        GameObject[] UI = GameObject.FindGameObjectsWithTag("UI_Item");

        for (int i = 0; i < UI.Length; i++)
        {
            Destroy(UI[i]);
        }

        if(MyItems != null)
            MyItems.Clear();

        if(MyKeyItems != null)
            MyKeyItems.Clear();
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

        // Si on sort des limites de l'inventaire par en les cotés
        if (option > ItemMax - 1)
            option = 0;
        if (option < 0)
            option = ItemMax - 1;

        // Affiche la description de l'objets selon si c'est une clé ou un consomable
        if (optInv == 0)
            DescriptionInventory(option);
        else
            DescriptionKeyInventory(option);

        //Positionne le curseur sur un objet selon les variables d'option
        Cursor.localPosition = new Vector2(posX + (option * 64), posY - (optInv == 0 ? 0 : 75));
    }
}
