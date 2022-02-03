using System.Collections;
using UnityEngine;

public class PlayerCC : MonoBehaviour {

    // Statistiques du personnage
    private static int Level;
    private static int MaxPv;
    private static int Pv;
    private static int MaxPm;
    private static int Pm;

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
    private bool inAir = true;
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
        Level = 1;

        MaxPv = 25; // + 55 + 20; // Max 100
        MaxPm = 20; // + 30; // Max 50

        Pv = MaxPv;
        Pm = MaxPm;

        Atk = 0 + (Sword == null ? 0 : Sword.Atk);

        Pause.SetActive(false);

        //An = GetComponent<Animator>();
        Cc = GetComponent<CharacterController>();

        GameObject.Find("UI_PV").GetComponent<RectTransform>().sizeDelta = new Vector2(16 + ((MaxPv / 10f) * 32) + 48 + 12, 24);
        GameObject.Find("UI_PM").GetComponent<RectTransform>().sizeDelta = new Vector2(16 + ((MaxPm / 10f) * 32) + 48 + 12, 24);

        PvFill = GameObject.Find("UI_PV_Fill");
        PvFill.GetComponent<RectTransform>().sizeDelta = new Vector2(((MaxPv / 10f) * 32), 18);
        PvFill.GetComponent<UI_PV>().UpdatePv((float)Pv / MaxPv);

        PmFill = GameObject.Find("UI_PM_Fill");
        PmFill.GetComponent<RectTransform>().sizeDelta = new Vector2(((MaxPm / 10f) * 32), 18);
        PmFill.GetComponent<UI_PV>().UpdatePv((float)Pm / MaxPm);

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
        if (Cc.isGrounded)
        {
            recovery.x = Mathf.Round(transform.position.x);
            recovery.y = Mathf.Round(transform.position.y);
            recovery.z = Mathf.Round(transform.position.z);

            inAir = false;

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
            if (!inAir && !Physics.Raycast(transform.position, Vector3.down * .025f, out _))
            {
                moveDirection.y = jumpSpeed;
                inAir = true;
            }
            Debug.DrawRay(transform.position, Vector3.down * 2f, Color.yellow);

        }

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
        Movable=!isPaused;
        SetOrientable(!isPaused);
        Pause.SetActive(isPaused);
        Pause.transform.GetChild(2).GetComponent<UI_Stat>().Actualisation();
        Pause.GetComponent<UI_Pause>().ActualisationInventaires();
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

    // Getter-Setter
    public static int GetLevel()
    {
        return Level;
    }

    public static int GetMaxPv()
    {
        return MaxPv;
    }
    
    public static int GetPv()
    {
        return Pv;
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
