using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Equipment: MonoBehaviour
{
    // Variable de gestion de l'inventaire
    private int option;
    private int optInv;
    private int cooldown;
    private int waiting;

    private int itCol;
    private int itRaw;

    private float inerTime;
    private float posX;
    private float posY;

    private Sprite NoImage;

    public GameObject UI_Item;

    // Gestion du personnage
    private PlayerCC PCC;
    private Weapon W;

    // Variables des éléments de UI du tableau d'Inventaire
    private Image ItemImage;
    private Text Description;
    private Text Stats;
    private Transform Cursor;

    // Liste des objets d'inventaire
    private List<Weapon> Weapons;
    private List<Protecter> Protecters;
    private List<Protecter> Necklaces;
    private List<Accessory> Accessories;

    // Start is called before the first frame update
    void Start()
    {
        option = 0;             // Choix dans l'inventaire  (position X)
        optInv = 0;             // Choix d'inventaire       (position Y)
        itCol = 5;              // Maximum d'objet dans l'inventaire par colone
        itRaw = 6;              // Maximum d'objet dans l'inventaire par rangée

        cooldown = 20;
        waiting = cooldown;
        inerTime = 0;

        // Récupere les élément de UI du tableau d'Inventaire
        ItemImage = transform.GetChild(9).GetComponent<Image>();
        Description = transform.GetChild(8).GetComponent<Text>();
        Stats = transform.GetChild(10).GetComponent<Text>();
        Cursor = transform.GetChild(11).transform;

        NoImage = ItemImage.GetComponent<Image>().sprite;
        Description.GetComponent<Text>().text = "";
        Stats.GetComponent<Text>().text = "";

        // Récupere la position du curseur
        posX = Cursor.localPosition.x;
        posY = Cursor.localPosition.y;

        Weapons = GameObject.Find("Player").GetComponent<InventoryCC>().GetWeapons();
        Protecters = GameObject.Find("Player").GetComponent<InventoryCC>().GetProtecters();
        Necklaces = GameObject.Find("Player").GetComponent<InventoryCC>().GetNecklaces();
        Accessories = GameObject.Find("Player").GetComponent<InventoryCC>().GetAccessories();

        PCC = GameObject.Find("Player").GetComponent<PlayerCC>();

        // Récupere l'équipement du personnage
        W = PCC.GetWeapon();


        for (int i = 0; i < Weapons.Count; i++)
        {
            //MyWeapons.Add(Weapons[i]);
            //MyWeapons[i].Qt = 1;
            Weapons[i].Qt = 1;
        }

        Weapons[1].Qt = 0;

        for (int i = 0; i < Protecters.Count; i++)
        {
            //MyProtecters.Add(Protecters[i]);
            //MyProtecters[i].Qt = 1;
            Protecters[i].Qt = i%2;
        }
        for(int i = 0; i < Accessories.Count; i++)
        {
            //MyAccessories.Add(Accessories[i]);
            Accessories[i].Qt = 1;
        }

        Weapons[3].Qt = 0;

        for (int i = 0; i < Necklaces.Count; i++)
        {
            //MyNecklaces.Add(Necklaces[i]);
            Necklaces[i].Qt = i%2;
        }

        DisplayWeapons();
        DisplayProtecters();
        DisplayNecklaces();
        DisplayAccessories();
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

        }
        if (Input.GetButtonDown("Fire2") && UI_Pause.option == 1)
        {
            Equip();
        }

        // Déplacement du curseur dans l'inventaire des objets consomables
        if ((Input.GetKeyDown(KeyCode.D) || (Input.GetAxis("Horizontal") >= 0.5f) && waiting == cooldown && UI_Pause.option == 1))
        {
            InventoryManager(1, 0);
        }
        else if ((Input.GetKeyDown(KeyCode.A) || (Input.GetAxis("Horizontal") <= -0.5f) && waiting == cooldown && UI_Pause.option == 1))
        {
            InventoryManager(-1, 0);
        }
        else if ((Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") >= 0.5f) && waiting == cooldown && UI_Pause.option == 1))
        {
            InventoryManager(0, -1);
        }
        else if ((Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("Vertical") <= -0.5f) && waiting == cooldown && UI_Pause.option == 1))
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

    // Affiche les armes dans la fenêtre d'équipement
    public void DisplayWeapons()
    {
        if (Weapons == null)
            return;

        GameObject g;

        int nbObjets = 3; // Nombre maximal d'armes affichables. Les 2 dernières sont des améliorations de la 3e.

        // Affiche les 2 premières armes
        for (int i = 0; i < nbObjets - 1; i++)
        {
            if (Weapons[i].Qt == 0)
                continue;

            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector3(posX + (i * 64), posY);
            g.name = "Weap" + i;
            g.GetComponent<Image>().sprite = Weapons[i].Sprite;
        }

        //Affiche la dernière selon certaines conditions.
        g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
        g.transform.SetParent(this.transform);
        g.transform.localScale = new Vector3(1, 1, 1);
        g.transform.localPosition = new Vector3(posX + (2 * 64), posY);
        g.name = "Weap" + 3;

        if (Weapons[4].Qt == 1)
            g.GetComponent<Image>().sprite = Weapons[4].Sprite;
        else if (Weapons[3].Qt == 1)
            g.GetComponent<Image>().sprite = Weapons[3].Sprite;
        else if (Weapons[2].Qt == 1)
            g.GetComponent<Image>().sprite = Weapons[2].Sprite;

    }

    // Affiche les armures dans la fenêtre d'équipement
    public void DisplayProtecters()
    {
        GameObject g;

        int nbObjets = Protecters.Count;

        for (int i = 0; i < nbObjets; i++)
        {
            if (Protecters[i].Qt == 0)
                continue;

            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector3(posX + (i * 64), posY - 80);
            g.name = "Protect" + i;
            g.GetComponent<Image>().sprite = Protecters[i].Sprite;
        }
    }

    // Affiche les colliers dans la fenêtre d'équipement
    public void DisplayNecklaces()
    {
        GameObject g;

        int nbObjets = Necklaces.Count;
        int k = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < itCol; j++)
            {
                if (k >= nbObjets)
                    break;

                if (Necklaces[j + (5 * i)].Qt == 0)
                    continue;

                g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
                g.transform.SetParent(this.transform);
                g.transform.localScale = new Vector3(1, 1, 1);
                g.transform.localPosition = new Vector3(posX + (j * 64), posY - 160 - (i * 64));
                g.name = "Acc" + i;
                g.GetComponent<Image>().sprite = Necklaces[j + (5 * i)].Sprite;
                k++;
            }
        }
    }

    // Affiche les Accessoires dans la fenêtre d'équipement
    public void DisplayAccessories()
    {
        GameObject g;

        int nbObjets = Accessories.Count;

        for (int i = 0; i < nbObjets; i++)
        {
            if (Accessories[i].Qt == 0)
                continue;

            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector3(posX + (i * 64), posY - 368);
            g.name = "Acc" + i;
            g.GetComponent<Image>().sprite = Accessories[i].Sprite;
        }
    }

    //Description des armes
    private void DescriptionWeapon(int i)
    {

        if (Weapons[i].Qt == 0 || i == -1 || Weapons == null || Weapons.Count == 0 || Weapons.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            Stats.text = "";
            return;
        }

        if(i == 2)
        {
            if (Weapons[4].Qt == 1)
            {
                Description.text = Weapons[4].Name + "\n\n" + Weapons[4].Description;
                ItemImage.sprite = Weapons[4].Sprite;
            }
            else if (Weapons[3].Qt == 1)
            {
                Description.text = Weapons[3].Name + "\n\n" + Weapons[3].Description;
                ItemImage.sprite = Weapons[3].Sprite;
            }
            else if (Weapons[2].Qt == 1)
            {
                Description.text = Weapons[2].Name + "\n\n" + Weapons[2].Description;
                ItemImage.sprite = Weapons[2].Sprite;
            }
        }
        else
        {
            Description.text = Weapons[i].Name + "\n\n" + Weapons[i].Description;
            ItemImage.sprite = Weapons[i].Sprite;
        }
        Stats.text = PCC.GetAtk() + (PCC.GetWeapon() == null ? 0 : PCC.GetWeapon().Atk) + " -> " + (PCC.GetAtk() + Weapons[i].Atk);
    }

    //Description des armures
    private void DescriptionProtecter(int i)
    {
        if (Protecters[i].Qt == 0 || i == -1 || Protecters == null || Protecters.Count == 0 || Protecters.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            Stats.text = "";
            return;
        }

        Description.text = Protecters[i].Name + "\n\n" + Protecters[i].Description;
        ItemImage.sprite = Protecters[i].Sprite;
        Stats.text = PlayerCC.GetMaxPv() + (PCC.GetArmor() == null ? 0 : PCC.GetArmor().PV) + " -> " + (PlayerCC.GetMaxPv() + Protecters[i].PV + "\n");
        Stats.text += PlayerCC.GetMaxPm() + (PCC.GetArmor() == null ? 0 : PCC.GetArmor().PM) + " -> " + (PlayerCC.GetMaxPm() + Protecters[i].PM);
    }

    //Description des colliers
    private void DescriptionNecklace(int i, int j)
    {

        j -= 2;

        if (Necklaces[i + (5 * j)].Qt == 0 || i == -1 || j == -1 || Necklaces == null || Necklaces.Count == 0 || Necklaces.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            Stats.text = "";
            return;
        }

        Description.text = Necklaces[i + (5 * j)].Name + "\n\n" + Necklaces[i + (5 * j)].Description;
        ItemImage.sprite = Necklaces[i + (5 * j)].Sprite;
        Stats.text = PlayerCC.GetMaxPv() + (PCC.GetNecklace() == null ? 0 : PCC.GetNecklace().PV) + " -> " + (PlayerCC.GetMaxPv() + Necklaces[i + (5 * j)].PV + "\n");
        Stats.text += PlayerCC.GetMaxPm() + (PCC.GetNecklace() == null ? 0 : PCC.GetNecklace().PM) + " -> " + (PlayerCC.GetMaxPm() + Necklaces[i + (5 * j)].PM);
    }

    //Description des Accessoires
    private void DescriptionAccessory(int i)
    {
        if (Accessories[i].Qt == 0 || i == -1 || Accessories == null || Accessories.Count == 0 || Accessories.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            return;
        }

        Description.text = Accessories[i].Name + "\n\n" + Accessories[i].Description;
        ItemImage.sprite = Accessories[i].Sprite;
    }

    public void DestroyInventory()
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

        byte margin = 0;


        // Si on sort des limites de l'inventaire par en haut ou par en bas
        if (optInv > itRaw - 1)
        {
            optInv = 0;
        }
        if (optInv < 0)
        {
            optInv = itRaw - 1;
        }

        // Si on sort des limites de l'inventaire par en les cotés
        if (option > itCol - 1)
        {
            option = 0;
        }
        if (option < 0)
        {
            option = itCol - 1;
        }

        // Si le curseur est dans les marges vides
        while ((optInv == 0 && option > 2) || (optInv < 2 && option == 4) || (optInv > 3 && option == 4))
        {
            if (x > 0)
                option = 0;
            else
                option--;
        }

        // Affiche la description de l'objets

        if (optInv == 0)        // Ligne des armes
            DescriptionWeapon(option);
        else if (optInv == 1)   // Ligne des armures
            DescriptionProtecter(option);
        else if (optInv == 5)   // Ligne des accessoires
            DescriptionAccessory(option);
        else                   // Lignes des colliers
            DescriptionNecklace(option, optInv);

        // Positionne le curseur sur un objet selon les variables d'option
        if (optInv == 1)
            margin = 1;
        else if (optInv == 5)
            margin = 3;
        else if(optInv > 1)
                margin = 2;

        Cursor.localPosition = new Vector2(posX + (option * 64), posY - (optInv * 64) - (margin * 16));
    }

    private void Equip()
    {
        switch (optInv)
        {
            case 0:
                PCC.SetWeapon(Weapons[option]);
                break;

            case 1:
                PCC.SetArmor(Protecters[option]);
                break;
        }
    }
}
