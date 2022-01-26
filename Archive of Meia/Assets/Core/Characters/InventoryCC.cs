using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCC : MonoBehaviour
{
    //Variables de fichiers et assets
    public TextAsset ItemsTextFile;
    public TextAsset KeyItemsTextFile;
    public TextAsset WeaAndProTextFile;
    public Sprite[] Sprites;

    //Variables textes des objets
    private string[,] ItemsStringTable;
    private string[,] KeyItemsStringTable;
    private string[,] WeaAndProStringTable;

    //argent
    private static int money = 0;

    //Liste de tout les objets disponibles en jeu
    private static List<Item> Items;
    private static List<Item> KeyItems;
    private static List<Weapon> Weapons;
    private static List<Protecter> Protecters;





    private static int[] IndexObjects;
    public static bool sacUnlocked;
    public static bool gunUnlocked;
    public static bool scaleUnlocked;
    public static bool lanterneUnlocked;
    public static bool stardustUnlocked;
    public static bool mapUnlocked;
    public static bool fmapUnlocked;

    //UI affichage des variables d'inventaire
    //private Text UI_Money;
    private UI_Key UI_Key;
    private UI_GoldKey UI_GoldKey;


    // Start is called before the first frame update
    void Start()
    {
        //Recupere le texte des fichiers
        if (ItemsTextFile == null || KeyItemsTextFile == null || WeaAndProTextFile == null)
        {
            Debug.Log("WARNING !! MISSING FILE IN PREFAB !!");
        }

        ItemsStringTable = FileTo2DString(ItemsTextFile);
        KeyItemsStringTable = FileTo2DString(KeyItemsTextFile);
        WeaAndProStringTable = FileTo2DString(WeaAndProTextFile);

        //Crée les liste d'objets
        if (Items == null)
            CreateObjects();

    }

    // Update is called once per frame
    void Update() { }

    //Gère la variable money, l'argent de l'inventaire
    public bool SetMoney(int amont) {

        if (money + amont < 0)
            return false;

        money += amont;

        return true;
    }

    public int GetMoney()
    {
        return money;
    }

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

    public Item GetItems(byte index)
    {
        if (index >= Items.Count)
            return null;

        return Items[index];
    }

    //Gère la liste des objets clés
    /*  0 = Clé       1 = Clé Or    2 = Carte     3 = Gd Carte     */
    /*  4 = Sac Dyn   5 = Lantern   6 = Pistol    7 - ... cf. doc  */
    public bool SetKeyItems(byte index, int amont)
    {
        if (index >= KeyItems.Count)
            return false;

        if (KeyItems[index].Qt + amont < 0)
            return false;

        KeyItems[index].Qt += amont;

        return true;
    }

    public Item GetKeyItems(byte index)
    {
        if (index >= Items.Count)
            return null;

        return KeyItems[index];
    }

    //Ajoute une clef à l'inventaire.
    public void AddKeys(int amount)
    {
        //key+=amount;
    }

    //public int Key { get => UI_Key.Keys; set => UI_Key.Keys = value; }

    //Ajoute une clef en or à l'inventaire.
    public bool GoldKey { get => UI_GoldKey.Key; set => UI_GoldKey.Key = value; }

    //Renvoie la liste d'objet disponible en jeu
    public List<Item> GetObjects()
    {
        return Items;
    }

    //Renvoie la quantité d'objet disponible en jeu
    public int[] GetIndexObjects()
    {
        return IndexObjects;
    }

    //Crée la liste d'objets
    public void CreateObjects()
    {
        int i = 1;
        Items = new List<Item>();
        KeyItems = new List<Item>();
        Weapons = new List<Weapon>();
        Protecters = new List<Protecter>();
        

        //Liste d'objets d'usage
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 50,    Sprites[0]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 90,    Sprites[1]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 140,   Sprites[2]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 75,    Sprites[3]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 250,   Sprites[4]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 100,   Sprites[5]));
        Items.Add(new Item(ItemsStringTable[TheGameManager.lang, i++], ItemsStringTable[TheGameManager.lang, i++], 1000,  Sprites[6]));

        i = 1;

        //Liste d'objets clés
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));
        KeyItems.Add(new Item(KeyItemsStringTable[TheGameManager.lang, i++], KeyItemsStringTable[TheGameManager.lang, i++], 0, Sprites[0]));

        i = 1;

        //Liste d'armes
        /* Épées | 0 -> 4 */
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 1, 0, Sprites[0]));
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 2, 0, Sprites[0]));
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 3, 0, Sprites[0]));
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 4, 0, Sprites[0]));
        Weapons.Add(new Weapon(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 5, 0, Sprites[0]));

        //Liste d'armures
        /* Colliers | 5 -> 20 */
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 1, 0, 10, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 2, 0, 60, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 3, 0, 100, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 4, 0, 150, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 5, 0, 180, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 6, 0, 200, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 7, 5, 250, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 8, 5, 290, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 8, 0, 310, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 10, 5, 350, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 11, 10, 400, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 12, 10, 450, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 13, 10, 500, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 14, 15, 0, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 15, 20, 0, Sprites[0]));
        /* Armures | 21 -> 25 */
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 0, 0, 10, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 1, 0, 80, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 2, 3, 160, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 3, 5, 0, Sprites[0]));
        Protecters.Add(new Protecter(WeaAndProStringTable[TheGameManager.lang, i++], WeaAndProStringTable[TheGameManager.lang, i++], 5, 10, 0, Sprites[0]));
        
    }

    public string[,] FileTo2DString(TextAsset File)
    {
        int x = 0;
        int y = 0;
        int l = 0;

        string textFile = File.ToString();
        string[] StringTable;
        string[,] TextTable;

        //Repaire toute les entrees du texte
        StringTable = textFile.Split(';', '\n');

        //Calcule le nombre de colones et de lignes
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
            for (int k = 0; k < ((x / y) + 1) ; k++)
            {
                TextTable[k, j] = StringTable[l];
                l++;

            }
        }

        return TextTable;
    }
}
