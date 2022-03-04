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

    private int equipID;

    private Sprite NoImage;

    public GameObject UI_Item;

    private Transform[] UI_EquipedCursor = new Transform[4];
    private static byte[] EquipCursorIndex = new byte[] { 99, 99, 99, 99 };

    // Gestion du personnage
    private PlayerCC PCC;
    private Weapon Sword;
    private Protecter Armor;
    private Protecter Necklace;
    private Accessory Accessory;

    // Variables des éléments de UI du tableau d'Inventaire
    private Image ItemImage;
    private Text Description;
    private Text Stats01;
    private Text Stats02;
    private Transform Cursor;
    private Color32 BaseColor;
    private Color32 RedColor;
    private Color32 BlueColor;

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
        Stats01 = transform.GetChild(10).GetComponent<Text>();
        Stats02 = transform.GetChild(11).GetComponent<Text>();
        Cursor = transform.GetChild(12).transform;

        NoImage = ItemImage.GetComponent<Image>().sprite;
        Description.GetComponent<Text>().text = "";
        Stats01.GetComponent<Text>().text = "";
        Stats02.GetComponent<Text>().text = "";

        BaseColor = Stats01.color;
        RedColor = new Color32(190, 80, 45, 255);
        BlueColor = new Color32(40, 180, 200, 255);

        // Récupere la position du curseur
        posX = Cursor.localPosition.x;
        posY = Cursor.localPosition.y;

        Weapons = GameObject.Find("Player").GetComponent<InventoryCC>().GetWeapons();
        Protecters = GameObject.Find("Player").GetComponent<InventoryCC>().GetProtecters();
        Necklaces = GameObject.Find("Player").GetComponent<InventoryCC>().GetNecklaces();
        Accessories = GameObject.Find("Player").GetComponent<InventoryCC>().GetAccessories();

        PCC = GameObject.Find("Player").GetComponent<PlayerCC>();

        // Récupere l'équipement du personnage
        Sword = PCC.GetWeapon();
        Armor = PCC.GetArmor();
        Necklace = PCC.GetNecklace();

        /*for (int i = 0; i < Weapons.Count; i++)
        {
            Weapons[i].Qt = 1;
        }
        //Weapons[4].Qt = 0;
        //Weapons[3].Qt = 0;
        Weapons[0].Qt = 0;

        for (int i = 0; i < Protecters.Count; i++)
        {
            Protecters[i].Qt = 1;
        }
        //Protecters[4].Qt = 0;
        //Protecters[3].Qt = 0;

        for (int i = 0; i < Accessories.Count; i++)
        {
            Accessories[i].Qt = 1;
        }

        //Weapons[3].Qt = 0;

        for (int i = 0; i < Necklaces.Count; i++)
        {
            Necklaces[i].Qt = 1;
        }

        //Necklaces[14].Qt = 0;*/

        DisplayWeapons();
        DisplayProtecters();
        DisplayNecklaces();
        DisplayAccessories();

        UI_EquipedCursor[0] = transform.GetChild(13).transform;
        UI_EquipedCursor[1] = transform.GetChild(14).transform;
        UI_EquipedCursor[2] = transform.GetChild(15).transform;
        UI_EquipedCursor[3] = transform.GetChild(16).transform;
        SetEquipCursor(false);

        Debug.Log("Equipé avec: " + (PCC.GetWeapon() != null ? PCC.GetWeapon().Name : "RIEN") + " + " + (PCC.GetArmor() != null ? PCC.GetArmor().Name : "RIEN"));
    }

    // Update is called once per frame
    void Update()
    {
        // Activer/Desactiver la pause
        if (Input.GetButtonDown("Jump"))
        {
            DestroyEquipment();
            PCC.TogglePauseGame();
        }

        if (Input.GetButtonDown("Fire1") && UI_Pause.option == 1)
        {

        }
        if (Input.GetButtonDown("Fire2") && UI_Pause.option == 1)
        {
            Equip();

            DisplayWeapons();
            DisplayProtecters();
            DisplayNecklaces();
            DisplayAccessories();
        }

        // Déplacement du curseur dans l'inventaire des objets.
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

        int nbObjets = 2; // Nombre maximal d'armes affichables. Les 2 dernières sont des améliorations de la 3e.

        // Affiche les 2 premières armes
        for (int i = 0; i < nbObjets; i++)
        {
            if (Weapons[i].Qt == 0)
                continue;

            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector2(posX + (i * 64), posY);
            g.name = "Weap" + i;
            g.GetComponent<Image>().sprite = Weapons[i].Sprite;
            g.transform.GetChild(0).GetComponent<Text>().text = "";
        }

        //Affiche la dernière selon certaines conditions.
        if (Weapons[2].Qt == 1 || Weapons[3].Qt == 1 || Weapons[4].Qt == 1)
        {
            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector3(posX + (2 * 64), posY);
            g.name = "Weap" + 3;
            g.transform.GetChild(0).GetComponent<Text>().text = "";

            if (Weapons[4].Qt == 1)
                g.GetComponent<Image>().sprite = Weapons[4].Sprite;
            else if (Weapons[3].Qt == 1)
                g.GetComponent<Image>().sprite = Weapons[3].Sprite;
            else if (Weapons[2].Qt == 1)
                g.GetComponent<Image>().sprite = Weapons[2].Sprite;
        }
        
    }

    // Affiche les armures dans la fenêtre d'équipement
    public void DisplayProtecters()
    {
        if (Protecters == null)
            return;

        int indexID = 0;
        GameObject g;

        // Affiche toutes les armures sauf la dernière.
        int nbObjets = Protecters.Count - 2;
        for (int i = 0; i < nbObjets; i++)
        {
            if (Protecters[i].Qt == 0)
                continue;

            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector2(posX + (i * 64), posY - 80);
            g.name = "Protect" + i;
            g.GetComponent<Image>().sprite = Protecters[i].Sprite;
            g.transform.GetChild(0).GetComponent<Text>().text = Protecters[i].Qt.ToString();
        }
        
        //Affiche la dernière selon certaines conditions.
        if (Protecters[3].Qt == 1 || Protecters[4].Qt == 1)
        {
            indexID = (Protecters[4].Qt == 1 ? 4 : 3);

            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector2(posX + (3 * 64), posY - 80);
            g.name = "Protect" + indexID;
            g.GetComponent<Image>().sprite = Protecters[indexID].Sprite;
            g.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }

    // Affiche les colliers dans la fenêtre d'équipement
    public void DisplayNecklaces()
    {
        GameObject g;

        int nbObjets = Necklaces.Count - 2;
        int k = 0;

        // Affiche touts les colliers sauf le dernier.
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
                g.transform.localPosition = new Vector2(posX + (j * 64), posY - 160 - (i * 64));
                g.name = "Acc" + i;
                g.GetComponent<Image>().sprite = Necklaces[j + (5 * i)].Sprite;
                g.transform.GetChild(0).GetComponent<Text>().text = Necklaces[j + (5 * i)].Qt.ToString();
                k++;
            }
        }

        //Affiche la dernière selon certaines conditions.
        if (Necklaces[13].Qt == 1 || Necklaces[14].Qt == 1)
        {
            int indexID = (Necklaces[14].Qt == 1 ? 14 : 13);

            g = Instantiate(UI_Item, Vector2.zero, new Quaternion());
            g.transform.SetParent(this.transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition = new Vector3(posX + (3 * 64), posY - 160 - (2 * 64));
            g.name = "Protect" + indexID;
            g.GetComponent<Image>().sprite = Necklaces[indexID].Sprite;
            g.transform.GetChild(0).GetComponent<Text>().text = "";
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
            g.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }

    //Description des armes
    private void DescriptionWeapon(int i)
    {
        if (Weapons[i].Qt == 0 || i == -1 || Weapons == null || Weapons.Count == 0 || Weapons.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            Stats01.text = PCC.GetAtk().ToString();
            Stats02.text = "";
            Stats01.color = BaseColor;
            return;
        }

        // Si le dernier équipement est dans l'inventaire il doit remplacer l'avant dernier.
        if (i == 2)
        {
            if (Weapons[4].Qt == 1)
            {
                equipID = 4;
            }
            else if (Weapons[3].Qt == 1)
            {
                equipID = 3;
            }
            else if (Weapons[2].Qt == 1)
            {
                equipID = i;
            }
        }
        else
        {
            equipID = i;
        }

        Description.text = Weapons[equipID].Name + "\n\n" + Weapons[equipID].Description;
        ItemImage.sprite = Weapons[equipID].Sprite;

        Stats01.text = PCC.GetAtk() + " - " + (Weapons[equipID].Atk);
        Stats02.text = "";

        if (Weapons[equipID].Atk > PCC.GetAtk())
            Stats01.color = BlueColor;
        else
            Stats01.color = (Weapons[equipID].Atk == PCC.GetAtk() ? BaseColor : RedColor);
    }

    //Description des armures
    private void DescriptionProtecter(int i)
    {
        if (Protecters[i].Qt == 0 || i == -1 || Protecters == null || Protecters.Count == 0 || Protecters.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            Stats01.text = PlayerCC.GetMaxPv().ToString();
            Stats02.text = PlayerCC.GetMaxPm().ToString();
            Stats01.color = BaseColor;
            Stats02.color = BaseColor;
            return;
        }

        // Si le dernier équipement est dans l'inventaire il doit remplacer l'avant dernier.
        if (i == 3)
        {
            if (Protecters[4].Qt >= 1)
            {
                equipID = 4;
            }
            else if(Protecters[3].Qt >= 1)
            {
                equipID = i;
            }
        }
        else
        {
            equipID = i;
        }

        int pv = PlayerCC.GetMaxPv() + Protecters[equipID].PV - (Armor == null ? 0 : Armor.PV);
        int pm = PlayerCC.GetMaxPm() + Protecters[equipID].PM - (Armor == null ? 0 : Armor.PM);

        Description.text = Protecters[equipID].Name + "\n\n" + Protecters[equipID].Description;
        ItemImage.sprite = Protecters[equipID].Sprite;

        Stats01.text = PlayerCC.GetMaxPv() + " - " + pv + " (" + Protecters[equipID].PV + ")";
        Stats02.text = PlayerCC.GetMaxPm() + " - " + pm + " (" + Protecters[equipID].PM + ")";
        
        if (pv > PlayerCC.GetMaxPv())
            Stats01.color = BlueColor;
        else
            Stats01.color = (pv == PlayerCC.GetMaxPv() ? BaseColor : RedColor);

        if (pm > PlayerCC.GetMaxPm())
            Stats02.color = BlueColor;
        else
            Stats02.color = (pm == PlayerCC.GetMaxPm() ? BaseColor : RedColor);
    }

    //Description des colliers
    private void DescriptionNecklace(int i, int j)
    {

        j -= 2;

        int k = i + (5 * j);

        if (Necklaces[k].Qt == 0 || i == -1 || j == -1 || Necklaces == null || Necklaces.Count == 0 || Necklaces.Count <= i)
        {
            Description.text = "";
            ItemImage.sprite = NoImage;
            Stats01.text = PlayerCC.GetMaxPv().ToString();
            Stats02.text = PlayerCC.GetMaxPm().ToString();
            Stats01.color = BaseColor;
            Stats02.color = BaseColor;
            return;
        }

        // Si le dernier équipement est dans l'inventaire il doit remplacer l'avant dernier.
        if (k == 13)
        {
            if (Necklaces[14].Qt >= 1)
            {
                equipID = 14;
            }
            else if (Necklaces[13].Qt >= 1)
            {
                equipID = k;
            }
        }
        else
        {
            equipID = k;
        }

        int pv = PlayerCC.GetMaxPv() + Necklaces[equipID].PV - (Necklace == null ? 0 : Necklace.PV);
        int pm = PlayerCC.GetMaxPm() + Necklaces[equipID].PM - (Necklace == null ? 0 : Necklace.PM);

        Description.text = Necklaces[equipID].Name + "\n\n" + Necklaces[equipID].Description;
        ItemImage.sprite = Necklaces[equipID].Sprite;

        Stats01.text = PlayerCC.GetMaxPv() + " - " + pv + " (" + Necklaces[equipID].PV + ")";
        Stats02.text = PlayerCC.GetMaxPm() + " - " + pm + " (" + Necklaces[equipID].PM + ")";

        if (pv > PlayerCC.GetMaxPv())
            Stats01.color = BlueColor;
        else
            Stats01.color = (pv == PlayerCC.GetMaxPv() ? BaseColor : RedColor);

        if (pm > PlayerCC.GetMaxPm())
            Stats02.color = BlueColor;
        else
            Stats02.color = (pm == PlayerCC.GetMaxPm() ? BaseColor : RedColor);
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

        Stats01.text = "";
        Stats02.text = "";

        Description.text = Accessories[i].Name + "\n\n" + Accessories[i].Description;
        ItemImage.sprite = Accessories[i].Sprite;
    }

    public void DestroyEquipment()
    {
        GameObject[] UI = GameObject.FindGameObjectsWithTag("UI_Equip");

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
        // TO DO
        // Quand on equipe une case vide il equipe la derniere arme ou armure survolée !! Corrigez ca !!

        switch (optInv)
        {
            case 0:
                if(Weapons[equipID] == null || Weapons[equipID].Qt == 0){
                    return;
                }

                Sword = Weapons[equipID];
                PCC.SetWeapon(Sword);
                PCC.ActualisationStats();
                DescriptionWeapon(option);
                break;

            case 1:
                if (Protecters[equipID] == null || Protecters[equipID].Qt == 0)
                {
                    return;
                }
                Armor = Protecters[equipID];
                PCC.SetArmor(Armor);
                PCC.ActualisationStats();
                DescriptionProtecter(option);
                break;

            case 5:
                //PCC.SetArmor(Protecters[option]);
                break;

            default:
                if (Necklaces[equipID] == null || Necklaces[equipID].Qt == 0)
                {
                    return;
                }
                Necklace = Necklaces[equipID];
                PCC.SetNecklace(Necklace);
                PCC.ActualisationStats();
                DescriptionNecklace(option, optInv);
                break;

        }
        transform.GetComponentInParent<UI_Pause>().ActualisationPanels();
        SetEquipCursor(true);
        Debug.Log("Equipé avec: \n" + (PCC.GetWeapon() != null ? PCC.GetWeapon().Name : "RIEN") + " + " + (PCC.GetArmor() != null ? PCC.GetArmor().Name : "RIEN") + " + " + (PCC.GetNecklace() != null ? PCC.GetNecklace().Name : "RIEN"));
    }

    // Positionne les curseurs d'inventaires selon l'équipement du joueur
    private void SetEquipCursor(bool Equip)
    {

        // Equipe l'élément selon l'Equip ID

        if (Equip)
        {
            switch (optInv)
            {
                case 0:
                    UI_EquipedCursor[0].localPosition = new Vector2(posX + ((equipID > 2 ? 2 : equipID) * 64), posY);
                    EquipCursorIndex[0] = byte.Parse((equipID > 2 ? 2 : equipID).ToString());
                    Debug.Log(new Vector2(posX + (equipID * 64), posY));
                    break;

                case 1:
                    UI_EquipedCursor[1].localPosition = new Vector2(posX + ((equipID > 3 ? 3 : equipID) * 64), posY - 80);
                    EquipCursorIndex[1] = byte.Parse((equipID > 3 ? 3 : equipID).ToString());
                    Debug.Log(new Vector2(posX + (equipID * 64), posY));
                    break;

                case 5:
                    // Pour les accerssoires. TO DO
                    break;

                default:
                    int x = ((equipID > 13 ? 13 : equipID) % 5) * 64;
                    int y = ((equipID/5) % 3) * 64;

                    Debug.Log((equipID > 13 ? 13 : equipID));
                    UI_EquipedCursor[2].localPosition = new Vector2(posX + x, posY - 160 - y);
                    EquipCursorIndex[2] = byte.Parse((equipID > 13 ? 13 : equipID).ToString());
                    break;
            }
        }
        else
        {
            if (EquipCursorIndex[0] != 99)
                UI_EquipedCursor[0].localPosition = new Vector2(posX + ((EquipCursorIndex[0] > 2 ? 2 : EquipCursorIndex[0]) * 64), posY);

            if (EquipCursorIndex[1] != 99)
                UI_EquipedCursor[1].localPosition = new Vector2(posX + ((EquipCursorIndex[1] > 3 ? 3 : EquipCursorIndex[1]) * 64), posY - 80);

            if (EquipCursorIndex[2] != 99)
            {
                int x = ((EquipCursorIndex[2] > 13 ? 13 : EquipCursorIndex[2]) % 5) * 64;
                int y = ((EquipCursorIndex[2] / 5) % 3) * 64;
                UI_EquipedCursor[2].localPosition = new Vector2(posX + x, posY - 160 - y);
            }
        }

    }
}
