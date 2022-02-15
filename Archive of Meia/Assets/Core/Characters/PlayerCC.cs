using System.Collections;
using UnityEngine;

public class PlayerCC : MonoBehaviour {

    // Statistiques du personnage
    private static int Level;
    private static int MaxPv;
    private static int Pv;
    private static int MaxPm;
    private static int Pm;

    private const int BASE_PV = 20;
    private const int BASE_PM = 19;

    private int Atk;

    // Gestion du temps
    private int time;
    private int cooldown;
      
    // Gestion des parametres physiques
    public float walkSpeed = 10f;
    public float jumpSpeed = 4f;
    public float gravity = 17f;

    public bool WorldMapCharacter = false;

    private bool canGetDammages = true;
    private bool inAir = false;
    private bool isPaused = false;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 recovery = Vector3.zero;

    private CharacterController Cc;
    private GameObject PvFill;
    private GameObject PmFill;
    private GameObject E;       //Epee
    private GameObject C;       //Character
    private GameObject P;       //Probe

    private GameObject InGame;
    private GameObject Pause;
    
    // Variable d'équipement du personnage
    private static Weapon Sword;
    private static Protecter Armor;
    private static Protecter Necklace;

    private void Awake()
    {
        InGame = GameObject.Find("InGame");
        Pause = GameObject.Find("Pause");
    }

    // Use this for initialization
    void Start ()
    {
        this.transform.position = TheGameManager.Dest;

        // Initialise le niveau qui commance à 0.
        Level = 1;

        // Initialise les points de vie et de magie avant d'être
        // augmenté par le niveau ou l'équipement.
        MaxPv = BASE_PV; // + 55 + 20; // Max 100
        MaxPm = BASE_PM; // + 30; // Max 50

        // Initialise l'attaque selon l'équipement
        Atk = 0 + (Sword == null ? 0 : Sword.Atk);

        // Désactive la pause
        Pause.SetActive(false);

        //An = GetComponent<Animator>();
        Cc = GetComponent<CharacterController>();

        ActualisationStats();

        // En début de jeu on commence avec le maximum de point.
        Pv = MaxPv;
        Pm = MaxPm;

        ActualisationUI();

        time = 15;
        cooldown = time;
        E = GameObject.Find("epee");
        C = GameObject.Find("Character");
        P = GameObject.Find("Probe");
        E.SetActive(false);
        Movable = true;

    }

    // Update is called once per frame
    void Update () {

        if (WorldMapCharacter)
        {
            probeBehaviour();
        }

        // ***
        // Déplacements
        // ***

        // Si le joueur est au sol on enregistre la dernière plateforme qu'il a en dessous de lui.
        if (Cc.isGrounded)
        {
            inAir = false;

            // Enregistre la position du dernier objet au sol pour en faire un point de résurection.
            RaycastHit hit;
            if (Physics.Raycast(P.transform.position, transform.TransformDirection(Vector3.down), out hit, 5))
            {
                recovery = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
            }

            //An.SetBool("jump", Input.GetButton("Jump"));
            Vector3 moveDirection2 = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.z = Input.GetAxis("Vertical");
            if (moveDirection2.magnitude > 1)
                moveDirection2.Normalize();
            moveDirection2 *= walkSpeed;
            moveDirection += moveDirection2;

            moveDirection = transform.TransformDirection(moveDirection);
        }
        else
        {
            // Si le personage ne touche plus le sol mais qu'il est au dessus d'une pente ou un escalier, alors on le colle au sol.
            if (!inAir && !Physics.Raycast(transform.position, Vector3.down * .025f, out _))
            {
                moveDirection.y = jumpSpeed;
                inAir = true;
            }

        }

        // Si le personnage est en dessous de -2 sur l'axe des Y, on le renvoit au point de  réapparition
        if(GetComponent<Transform>().position.y <= -2)
        {
            transform.position = recovery;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        if(Movable)
            Cc.Move(moveDirection * Time.deltaTime);

        // ***
        // Coups d'épée
        // ***
        if (Input.GetButtonDown("Fire3") && cooldown == time )
        {
            cooldown = 0;
            E.SetActive(true);
            C.GetComponent<PlayerOrientation>().SetOrientable(false);
            Movable = false;

        }

        if (cooldown == time - 1)
        {
            Movable = true;
            C.GetComponent<PlayerOrientation>().SetOrientable(true);
        }

        if (cooldown == (int) time/3)
        {
            E.SetActive(false);
        }

        if (cooldown < time )
        {
            cooldown++;
        }

        if (Input.GetButtonDown("Jump"))
        {
            TogglePauseGame();
        }

        if (Pv > MaxPv){
            Pv=MaxPv;
        }
    }

    // ***
    // Active la pause
    // ***
    public void TogglePauseGame()
    {
        isPaused = !isPaused; // inversion

        InGame.SetActive(!isPaused);
        Time.timeScale = (isPaused ? 0 : 1);
        Movable = !isPaused;
        SetOrientable(!isPaused);
        Pause.SetActive(isPaused);
        Pause.GetComponent<UI_Pause>().ActualisationPanels();

        if (!isPaused)
            ActualisationUI();
        else
            Pause.transform.GetChild(2).GetComponent<UI_Stat>().Actualisation();

    }

    public void SetOrientable(bool p)
    {
        C.GetComponent<PlayerOrientation>().SetOrientable(p);
    }

    public void Hit(int amount)
    {
        if (canGetDammages)
        {
            Pv -= amount;
            canGetDammages = false;
            PvFill.GetComponent<UI_PV>().UpdatePv((float) Pv/MaxPv);
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
            StartCoroutine(recoveryTime(2));
            Cc.Move(transform.GetChild(0).transform.rotation * Vector3.back * 1.5f);
        }
    }

    public void Heal(int amount)
    {
        Pv += amount;
        if (Pv > MaxPv)
            Pv = MaxPv;
        PvFill.GetComponent<UI_PV>().UpdatePv((float)Pv / MaxPv);
    }

    IEnumerator recoveryTime(float p)
    {
        yield return new WaitForSeconds(p);
        canGetDammages = true;
        transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetButtonDown("Fire3") && (other.tag == "Ennemi" && cooldown <= 5))
        {
            Debug.Log(other);
            other.GetComponent<FoeBehaviour>().Dammages(1);
        }
    }

    private void probeBehaviour()
    {
        RaycastHit hit;
        if (Physics.Raycast(P.transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(P.transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            Debug.Log(hit.transform.name);
            if(hit.transform.name == "Ocean")
            {
                Movable = false;
            }
            else
            {
                Movable = true;
            }
        }
    }

    // Actualise la vie et la magie du personnage selon le niveau et l'équipement.
    public void ActualisationStats()
    {
        Atk = (Sword == null ? 0 : Sword.Atk);
        MaxPv = BASE_PV + (Level * 5) + (Armor == null ? 0 : Armor.PV) + (Necklace == null ? 0 : Necklace.PV);
        MaxPm = BASE_PM + Level + (Armor == null ? 0 : Armor.PM) + (Necklace == null ? 0 : Necklace.PM);
    }

    // Actualise le UI selon les stats du personnage.
    public void ActualisationUI()
    {
        GameObject.Find("UI_PV").GetComponent<RectTransform>().sizeDelta = new Vector2(16 + ((MaxPv / 10f) * 32) + 48 + 12, 24);
        GameObject.Find("UI_PM").GetComponent<RectTransform>().sizeDelta = new Vector2(16 + ((MaxPm / 10f) * 32) + 48 + 12, 24);

        PvFill = GameObject.Find("UI_PV_Fill");
        PvFill.GetComponent<RectTransform>().sizeDelta = new Vector2(((MaxPv / 10f) * 32), 18);
        PvFill.GetComponent<UI_PV>().UpdatePv((float)Pv / MaxPv);

        PmFill = GameObject.Find("UI_PM_Fill");
        PmFill.GetComponent<RectTransform>().sizeDelta = new Vector2(((MaxPm / 10f) * 32), 18);
        PmFill.GetComponent<UI_PV>().UpdatePv((float)Pm / MaxPm);
    }

    // Getter-Setter
    public static int GetLevel()
    {
        return Level;
    }

    public static int GetBASEPV()
    {
        return BASE_PV;
    }

    public static int GetMaxPv()
    {
        return MaxPv;
    }
    
    public static int GetPv()
    {
        return Pv;
    }

    public static int GetBASEPM()
    {
        return BASE_PM;
    }

    public static int GetMaxPm()
    {
        return MaxPm;
    }

    public static int GetPm()
    {
        return Pm;
    }

    public int GetAtk()
    {
        return Atk;
    }

    // Set/Get l'arme
    public void SetWeapon(Weapon W)
    {
        Sword = W;
    }
    public Weapon GetWeapon()
    {
        return Sword;
    }

    // Set/Get l'armure
    public void SetArmor(Protecter P)
    {
        Armor = P;
    }
    public Protecter GetArmor()
    {
        return Armor;
    }

    // Set/Get le collier
    public void SetNecklace(Protecter N)
    {
        Necklace = N;
    }
    public Protecter GetNecklace()
    {
        return Necklace;
    }

    public bool Movable { get; set; }

}
