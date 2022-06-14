using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.U2D;

public class InventoryCC : MonoBehaviour
{
    //Variables de fichiers et assets
    public TextAsset ItemsTextFile;
    public TextAsset KeyItemsTextFile;
    public TextAsset WeaAndProTextFile;
    public TextAsset AccessoriesTextFile;
    public Sprite[] Sprites;

    // Variables textes des objets
    private string[,] ItemsStringTable;
    private string[,] KeyItemsStringTable;
    private string[,] WeaAndProStringTable;
    private string[,] AccessoriesStringTable;

    // argent
    private static int money;

    // Liste de tout les objets disponibles en jeu
    private static List<Item> Items;
    private static List<Item> KeyItems;
    private static List<Weapon> Weapons;
    private static List<Protecter> Protecters;
    private static List<Protecter> Necklaces;
    private static List<Accessory> Accessories;

    // UI affichage des variables d'inventaire
    private Text UI_Money;
    private Text UI_Key;
    private Image I_GoldKey;
    
    //Start is called before the first frame update
    void Start()
    {
        // Recupere le texte des fichiers
        if (ItemsTextFile == null || KeyItemsTextFile == null || WeaAndProTextFile == null || AccessoriesTextFile == null)
        {
            Debug.Log("WARNING !! MISSING FILE IN PREFAB !!");
        }

        ItemsStringTable = FileTo2DString(ItemsTextFile);
        KeyItemsStringTable = FileTo2DString(KeyItemsTextFile);
        WeaAndProStringTable = FileTo2DString(WeaAndProTextFile);
        AccessoriesStringTable = FileTo2DString(AccessoriesTextFile);

        //Crée les liste d'objets
        if (Items == null)
            CreateObjects();

        // Récupère les objets texts de UI
        UI_Money = GameObject.Find("UI_Money").GetComponent<Text>();
        UI_Key = GameObject.Find("UI_Key").GetComponent<Text>();
        I_GoldKey = GameObject.Find("I_GoldKey").GetComponent<Image>();

        // Met à jour l'argent
        UI_Money.text = money.ToString();
        UI_Key.text = GetKeyItem(0).ToString();

        // Met à jour les clés
        UI_Key.text = GetKeyItem(0).Qt.ToString();

        // Met à jour les clés en or
        if (GetKeyItem(0).Qt > 0)
            I_GoldKey.color = Color.white;
        else
            I_GoldKey.color = Color.clear;

    }

    // Update is called once per frame
    void Update() { }

    // Gère la variable money, l'argent de l'inventaire
    public bool SetMoney(int amont) {

        if (money + amont < 0)
            return false;

        money += amont;

        // Met à jour l'argent du joueur
        UI_Money.text = money.ToString();

        return true;
    }

    // Rend l'argent, pas comme Mélenchon
    public int GetMoney()
    {
        return money;
    }

    //
    //  OBJETS  //
    //

    //Gère la liste d'objets d'usage
    /*  0 = Pomme V   1 = Pomme J   2 = Pomme R   3 = Carambole     */
    /*  4 = SaladeF   5 = Carotte   6 = Piment    7 = Dynamite      */
    public bool SetItems(byte index, int amont)
    {
        if (index >= Items.Count)
            return false;

        if (Items[index].Qt + amont < 0)
            return false;

        Items[index].Qt += amont;

        return true;
    }

    // Renvoie l'objet selon l'index
    public Item GetItems(byte index)
    {
        if (index >= Items.Count)
            return null;

        return Items[index];
    }

    // Renvoie la liste d'objets disponibles en jeu
    public List<Item> GetItems()
    {
        return Items;
    }

    //
    //  OBJETS CLÉ //
    //

    //Gère la liste des objets clés
    /*  0 = Clé       1 = Clé Or    2 = Dynami    3 = Crystal      */
    /*  4 = Écaille   5 = Poudre    6 = Orbe R    7 = Orbe B       */
    /*  8 = Orbe M    9 = Clef      10 = Carte    11 = Gd Carte    */
    /*  12 = Gd bourse (PAS ENCORE IMPLEMENTE) */
    public bool SetKeyItems(byte index, int amont)
    {
        if (index >= KeyItems.Count)
            return false;

        if (KeyItems[index].Qt + amont < 0)
            return false;

        KeyItems[index].Qt += amont;

        if (index == 0) // Si l'objet est une clé (index 0)
            UI_Key.text = GetKeyItem(0).Qt.ToString();

        if (index == 1) // Si l'objet est une clé en Or (index 1)
            if (GetKeyItem(0).Qt > 0)
                I_GoldKey.color = Color.white;
            else
                I_GoldKey.color = Color.clear;


        return true;
    }

    // Renvoie l'objet clé selon l'index
    public Item GetKeyItem(byte index)
    {
        if (index >= Items.Count)
            return null;

        return KeyItems[index];
    }

    // Renvoie la liste d'objets clés disponibles en jeu
    public List<Item> GetKeyItems()
    {
        return KeyItems;
    }

    //
    //  Armes  //
    //

    //Gère la liste des armes
    /*  0 = Épée       1 = Renegat     2 = Sublima nv1   */
    /*3 = Sublima nv2  4 = Sublima nv3                   */
    public bool SetWeapons(byte index, int amont)
    {
        if (index >= Weapons.Count)
            return false;

        if (Weapons[index].Qt + amont < 0)
            return false;

        Weapons[index].Qt += amont;

        return true;
    }

    // Renvoie l'arme selon l'index
    public Weapon GetWeapon(byte index)
    {
        if (index >= Weapons.Count)
            return null;

        return Weapons[index];
    }

    // Renvoie la liste d'armes disponibles en jeu
    public List<Weapon> GetWeapons()
    {
        return Weapons;
    }

    //
    // Armures //
    //

    //Gère la liste des armes
    public bool SetProtecters(byte index, int amont)
    {
        if (index >= Protecters.Count)
            return false;

        if (Protecters[index].Qt + amont < 0)
            return false;

        Protecters[index].Qt += amont;

        return true;
    }

    // Renvoie l'arme selon l'index
    public Protecter GetProtecter(byte index)
    {
        if (index >= Weapons.Count)
            return null;

        return Protecters[index];
    }

    // Renvoie la liste d'armes disponibles en jeu
    public List<Protecter> GetProtecters()
    {
        return Protecters;
    }

    //
    // Colliers //
    //

    //Gère la liste des collier
    public bool SetNecklaces(byte index, int amont)
    {
        if (index >= Necklaces.Count)
            return false;

        if (Necklaces[index].Qt + amont < 0)
            return false;

        Necklaces[index].Qt += amont;

        return true;
    }

    // Renvoie le collier selon l'index
    public Protecter GetNecklace(byte index)
    {
        if (index >= Weapons.Count)
            return null;

        return Necklaces[index];
    }

    // Renvoie la liste de colliers disponibles en jeu
    public List<Protecter> GetNecklaces()
    {
        return Necklaces;
    }

    //
    // Accessoires //
    //

    //Gère la liste des accessoires
    public bool SetAccessories(byte index, int amont)
    {
        if (index >= Accessories.Count)
            return false;

        if (Accessories[index].Qt + amont < 0)
            return false;

        Accessories[index].Qt += amont;

        return true;
    }

    // Renvoie l'accessoires selon l'index
    public Accessory GetAccessory(byte index)
    {
        if (index >= Weapons.Count)
            return null;

        return Accessories[index];
    }

    // Renvoie la liste d'accessoires disponibles en jeu
    public List<Accessory> GetAccessories()
    {
        return Accessories;
    }


    // Crée la liste d'objets
    public void CreateObjects()
    {
        int i = 1;
        Items = new List<Item>();
        KeyItems = new List<Item>();
        Weapons = new List<Weapon>();
        Necklaces = new List<Protecter>();
        Protecters = new List<Protecter>();
        Accessories = new List<Accessory>();
        

        // Liste d'objets d'usage
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 50,    Sprites[0]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 90,    Sprites[1]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 140,   Sprites[2]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 75,    Sprites[3]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 250,   Sprites[4]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 80,    Sprites[5]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 120,   Sprites[6]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 1000,  Sprites[7]));

        i = 1;

        // Liste d'objets clés
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[8]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[9]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[10]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[11]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[12]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[13]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[14]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[15]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[16]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[17]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[18]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[19]));

        i = 1;

        // Liste d'armes
        /* Épées | 0 -> 4 */
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 1, 0, Sprites[2]));
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 2, 0, Sprites[2]));
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 3, 0, Sprites[2]));
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 4, 0, Sprites[2]));
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 5, 0, Sprites[6]));

        // Liste de colliers
        /* Colliers | 5 -> 20 */
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 1, 0, 10, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 2, 0, 60, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 3, 0, 100, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 4, 0, 150, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 5, 0, 180, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 6, 0, 200, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 7, 5, 250, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 8, 5, 290, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 9, 0, 310, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 10, 5, 350, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 11, 10, 400, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 12, 10, 450, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 13, 10, 500, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 14, 15, 0, Sprites[0]));
        Necklaces.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 15, 20, 0, Sprites[6]));

        // Liste d'armures
        /* Armures | 21 -> 25 */
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 0, 0, 10,  Sprites[1]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 2, 0, 80,  Sprites[1]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 5, 5, 160, Sprites[1]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 8, 8, 0,   Sprites[1]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 10, 10, 0,  Sprites[6]));

        i = 1;

        // Liste d'accessoires
        Accessories.Add(new Accessory(AccessoriesStringTable[TheGameManager.lang, i++], AccessoriesStringTable[TheGameManager.lang, i++], Sprites[5]));
        Accessories.Add(new Accessory(AccessoriesStringTable[TheGameManager.lang, i++], AccessoriesStringTable[TheGameManager.lang, i++], Sprites[5]));
        Accessories.Add(new Accessory(AccessoriesStringTable[TheGameManager.lang, i++], AccessoriesStringTable[TheGameManager.lang, i++], Sprites[5]));
        Accessories.Add(new Accessory(AccessoriesStringTable[TheGameManager.lang, i++], AccessoriesStringTable[TheGameManager.lang, i++], Sprites[5]));
    }

    public string[,] FileTo2DString(TextAsset File)
    {
        int x = 0;
        int y = 0;
        int l = 0;

        string textFile = File.ToString();
        string[] StringTable;
        string[,] TextTable;

        // Repaire toute les entrees du texte
        StringTable = textFile.Split(';', '\n');

        // Calcule le nombre de colones et de lignes
        for (int i = 0; i < File.ToString().Length; i++)
        {
            if (textFile[i] == '\n')
                y++;
            if (textFile[i] == ';')
                x++;
        }

        // Declare un tableau selon les ligne et les colones
        TextTable = new string[(x / y) + 1, y];

        // Remplis le tableau avec les entrees de textes
        for (int j = 0; j < y; j++)
        {
            for (int k = 0; k < ((x / y) + 1) ; k++)
            {
                TextTable[k, j] = StringTable[l];
                l++;
            }
        }

        return TextTable;
    }
}
