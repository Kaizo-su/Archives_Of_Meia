using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCC : MonoBehaviour
{
    private static int monnaie = 0;

    private UI_Money UI_Money;
    private UI_Key UI_Key;
    private UI_GoldKey UI_GoldKey;

    public Sprite[] Sprites;

    private static List<Item> Objects;
    private static int[] IndexObjects;

    public static bool sacUnlocked;
    public static bool gunUnlocked;
    public static bool scaleUnlocked;
    public static bool lanterneUnlocked;
    public static bool stardustUnlocked;
    public static bool mapUnlocked;
    public static bool fmapUnlocked;


    //public static int Key { get => key; set => key = value; }
    //public int Monnaie { get => monnaie; set { monnaie = value; UI_Money.UpdateMoney(monnaie); } }

// Start is called before the first frame update
void Start()
    {
        UI_Money = GameObject.Find("UI_Money").GetComponent<UI_Money>();
        UI_Key = GameObject.Find("UI_Key").GetComponent<UI_Key>();
        UI_GoldKey = GameObject.Find("I_GoldKey").GetComponent<UI_GoldKey>();

        if(Objects == null)
            CreateObjects();
    }

    // Update is called once per frame
    void Update() { }
    public int Money {
        set {
            monnaie = value;
            UI_Money.UpdateMoney(monnaie);
        }
        get => monnaie;
    }

    //Ajoute une clef à l'inventaire.
    public void AddKeys(int amount)
    {
        Key+=amount;
    }

    public int Key { get => UI_Key.Keys; set => UI_Key.Keys = value; }

    //Ajoute une clef en or à l'inventaire.
    public bool GoldKey { get => UI_GoldKey.Key; set => UI_GoldKey.Key = value; }

    //Ajoute des objets dans l'inventaire
    public void AddObject(int p)
    {
        if(p < Sprites.Length)
        {
            Debug.Log("VIDE !!");
            return;
        }
        IndexObjects[p]++;
    }

    //Renvoie la liste d'objet disponible en jeu
    public List<Item> GetObjects()
    {
        return Objects;
    }

    //Renvoie la quantité d'objet disponible en jeu
    public int[] GetIndexObjects()
    {
        return IndexObjects;
    }

    //Crée la liste d'objets
    public void CreateObjects() 
    {
        //Debug.Log( GameObject.Find("Player").GetComponent<UI_Inventory>().Sprites);
        Objects = new List<Item>();
        //Sprite[] UnityComeOn = GameObject.Find("Player").GetComponent<UI_Inventory>().Sprites;
        //Objects.Add(new Item("Potion", "Une mixture qui rend 25 points de vie, et qui goûte un peu la fraise.", 50, Sprites[0]));
        Objects.Add(new Item("Super Potion", "Une mixture qui rend 50 points de vie, et qui goûte la menthe.", 95, Sprites[1]));
        Objects.Add(new Item("Hyper Potion", "Une mixture qui rend tous les points de vie, et qui goûte le chocolat.", 190, Sprites[2]));
        Objects.Add(new Item("Eau forte", "Un liquide vaporeux qui rend toute la magie et qui goute le vieux sucre.", 150, Sprites[3]));
        Objects.Add(new Item("Antidote", "Un produit qui soigne les altérations d'états. Ça sent le mauvais citron.", 75, Sprites[4]));
        Objects.Add(new Item("Élixire de vie", "Un élixire parfumée qui vous ramène à la vie en cas de coups fatale. Ça sent la grenade.", 1000, Sprites[5]));
        Objects.Add(new Item("1", "JE SUIS UN TEST", 0, Sprites[0]));
        Objects.Add(new Item("2", "JE SUIS UN TEST", 0, Sprites[0]));
        Objects.Add(new Item("3", "JE SUIS UN TEST", 0, Sprites[0]));

        if(sacUnlocked){
            Objects.Add(new Item("Bombe", "Un contenant en argile rempli de poudre. Permet de brisser des murs fragiles.", 15, Sprites[6]));
            Objects.Add(new Item("Sac de Bombes", "Permet de placer une bombe à l’endroit où vous êtes. Ne rester pas sur place trop longtemps car elle va vite exploser !", 0, Sprites[7]));
        }
        if(lanterneUnlocked){
            Objects.Add(new Item("Lanterne", "Permet d’illuminer les alentours et même de révéler la présence d’objet tapis dans l’obscurité. Elle consomme de la magie pour briller.", 0,Sprites[8]));
        }
        if(gunUnlocked){
            Objects.Add(new Item("Pistolet à foudre", "Un mousquet à silex développé pour lancer des perles de foudre. Utile contre les monstres ou pour activer des mécanismes électriques.", 0, Sprites[9]));
            Objects.Add(new Item("Crystal terne", "Une étrange pierre précieuse. Son éclat doit être ravive pour qu’elle recouvre ses pouvoirs.", 0,Sprites[10]));
            Objects.Add(new Item("Crystal magique", "Une étrange pierre précieuse qui vous protège des attaques en échange de votre magie.", 0, Sprites[11]));
        }
        if(scaleUnlocked){
            Objects.Add(new Item("Ecaille d’argent", "Des écailles de sirène appartenant à la sirène d’argent cachée quelque part en Méia.", 0, Sprites[12]));
        }
        if(stardustUnlocked){
            Objects.Add(new Item("Poudre d’étoile", "De la poussière de minerais rare utilisé en alchimie pour renforcer l’équipement.", 0, Sprites[13]));
        }
        if(mapUnlocked){
            Objects.Add(new Item("Carte du monde", "Une carte des îles qui représentent la mer intérieure et les 6 îles principales du monde connu.", 0, Sprites[14]));
        }
        if(fmapUnlocked){
            Objects.Add(new Item("Carte complète du monde", "Une carte unique en son genre qui représente la mer intérieur et extérieur, ainsi que les 12 îles de Méia.", 0, Sprites[15]));
        }
        


        /*Objects.Add(new Item("1", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("2", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("3", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("4", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("5", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("6", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("7", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("8", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("9", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("10", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("11", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("12", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("13", "JE SUIS UN TEST", 0));
        Objects.Add(new Item("14", "JE SUIS UN TEST", 0));*/

        IndexObjects = new int[Objects.Count];
    }
}
