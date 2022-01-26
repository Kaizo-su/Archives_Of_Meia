using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private int option;
    private int cooldown;
    private int waiting;
    private int objetMax;
    private int hiddenObject;

    private float posX;
    private float posY;

    public GameObject UI_Item;

    private PlayerCC PCC;
    private Text Description;
    private Transform Cursor;

    private List<Item> Objects;
    private List<Item> ObjectsInPossession;
    private int[] IndexObjects;

    private GameObject Pause;
   //private MonoBehaviour playerScript;

    // Start is called before the first frame update
    void Start()
    {
        option = 0;
        cooldown = 12;
        waiting = cooldown;
        objetMax = 7;
        hiddenObject = 0;


        Description = transform.GetChild(1).GetComponent<Text>();
        
        Cursor = transform.GetChild(3).transform;
        posX = Cursor.localPosition.x;
        posY = Cursor.localPosition.y;

        //UpArrow = transform.GetChild(4).GetComponent<Image>();
        //DownArrow = transform.GetChild(5).GetComponent<Image>();

        Objects = GameObject.Find("Player").GetComponent<InventoryCC>().GetObjects();
        PCC = GameObject.Find("Player").GetComponent<PlayerCC>();

        DescriptionInventaire(option);

        //playerScript = GameObject.Find("Player").GetComponent<PlayerCC>();
        Pause = GameObject.Find("Pause");

        //FOR TESTING , remplie un peu l'inventaire
        /*IndexObjects = GameObject.Find("Player").GetComponent<InventoryCC>().GetIndexObjects();
        IndexObjects[0] = 3;
        IndexObjects[1] = 2;
        IndexObjects[2] = 2;
        IndexObjectsSaved = IndexObjects;*/
        DisplayInventory();

        InventoryCC.sacUnlocked = true;
        UnlockItem();
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
        if (Input.GetButtonDown("Fire2"))
        {

            //IndexObjects[option]++;
            DestroyInventory();
            DisplayInventory();

        }

        // Déplacement du currseur dans l'inventaire des objets consomables
        if (Input.GetKeyDown(KeyCode.W) && UI_Pause.option== 3 || (Input.GetAxis("Horizontal") >= 0.5f && waiting == cooldown && UI_Pause.option== 3))
        {
            option++;
            waiting = 0;
            option += objetMax;
            option %= objetMax;
            DescriptionInventaire(option);
        }
        else if (Input.GetKeyDown(KeyCode.S) && UI_Pause.option== 3  || (Input.GetAxis("Horizontal") <= -0.5f && waiting == cooldown && UI_Pause.option== 3))
        {
            option--;
            waiting = 0;
            option += objetMax;
            option %= objetMax;
            DescriptionInventaire(option);
        }

        

        if (waiting < cooldown){
            waiting++;
        }

        Cursor.localPosition = new Vector2(posX + (option * 90), posY);
    }

    public void DisplayInventory()
    {

        Cursor.localPosition = new Vector3(posX + 120, posY, 0);

        GameObject g;
        IndexObjects = GameObject.Find("Player").GetComponent<InventoryCC>().GetIndexObjects();

        int nbObjets = (Objects.Count < objetMax ? Objects.Count : objetMax);
        
        for (int i = 0; i < nbObjets; i++)
        {
                g = Instantiate(UI_Item, Vector3.zero, new Quaternion());
                g.transform.SetParent(this.transform);
                g.transform.localScale = new Vector3(1, 1, 1);
                g.transform.localPosition = new Vector3(posX + (i * 90f), posY);
                g.name = "Item" + i + hiddenObject;
                g.GetComponent<Image>().sprite = Objects[i].Sprite;
                g.transform.GetChild(0).GetComponent<Text>().text = Objects[i].Name;
                g.transform.GetChild(1).GetComponent<Text>().text = Objects[i].Qt.ToString();
        }
        
    }

    private void DescriptionInventaire(int i)
    {
        /*if (IndexObjects == null || IndexObjects[i] == 0)
            Description.text = "";
        else
            Description.text = Objects[i].GetName() + "\n\n" + Objects[i].GetDescription();*/
        /*
        if (Objects.Count > i){

            if (Objects[i].Qt>0){
                Description.text = Objects[i].Name + "\n\n" + Objects[i].Description;
            }else {
                //Description.text= " ";
                //Description.text = Objects[i].Name + "\n\n" + Objects[i].Description; //JUST POUR DEVELOPPEMENT SO I KNOW WTF
            }

        }else {

            Debug.Log("Dude I don't know");
        }   */
        Debug.Log(Objects[i].Name);

        Description.text = Objects[i].Name + "\n\n" + Objects[i].Description;
    }

    private void DestroyInventory()
    {
        GameObject[] UI = GameObject.FindGameObjectsWithTag("UI_Item");

        for (int i = 0; i < UI.Length; i++)
        {
            Destroy(UI[i]);
        }
    }

    private void InPossessionObjects()
    {
        ObjectsInPossession.Clear();

        for (int i = 0; i < IndexObjects.Length; i++)
        {
            if(IndexObjects[i] > 0)
            {
                ObjectsInPossession.Add(Objects[i]);
            }
        }
    }

    public void UnlockItem(){

        InventoryCC.sacUnlocked = true;
        GameObject.Find("Player").GetComponent<InventoryCC>().CreateObjects();
        Objects = GameObject.Find("Player").GetComponent<InventoryCC>().GetObjects();
        IndexObjects = GameObject.Find("Player").GetComponent<InventoryCC>().GetIndexObjects();
        /*
        for(int i = 0; i < IndexObjectsSaved.Length; i++){

            IndexObjects[i] = IndexObjectsSaved[i];
        }
        
        if (InventoryCC.sacUnlocked){
            for(int i = 0; i < IndexObjects.Length; i++){
                string Nom = Objects[i].Name;
                if(Nom== "Sac de Bombes"){
                    IndexObjects[i] = 1;
                }
            }
        }
        IndexObjectsSaved = IndexObjects; */
        DestroyInventory();
        DisplayInventory();


    }
}
