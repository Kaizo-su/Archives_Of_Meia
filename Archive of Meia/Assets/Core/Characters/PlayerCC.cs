using System.Collections;
using UnityEngine;

public class PlayerCC : MonoBehaviour {

    private int time;
    private int cooldown;
    private int actionCooldown;
      
    public float walkSpeed = 10f;
    public float jumpSpeed = 4f;
    public float gravity = 17f;

    private bool canGetDammages = true;
    private bool inAir = true;
    private bool movable = true;
    private bool isPaused = false;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 recovery = Vector3.zero;

    private CharacterController Cc;
    private GameObject PvFill;
    private GameObject PmFill;
    private GameObject E;
    private GameObject C;

    private GameObject InGame;
    private GameObject Pause;

    private void Awake()
    {
        InGame = GameObject.Find("InGame");
        Pause = GameObject.Find("Pause");

        Pause.SetActive(false);
    }

    // Use this for initialization
    void Start ()
    {
        Level = 1;

        MaxPv = 25; // + 55 + 20; // Max 100
        MaxPm = 20; // + 30; // Max 50

        Pv = MaxPv;
        Pm = MaxPm;

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
        actionCooldown = 0;
        E = GameObject.Find("epee");
        C = GameObject.Find("Character");

    }

    void FixedUpdate()
    {
        if (actionCooldown >= 0)
        { //Cooldown sur l'attaque
            actionCooldown--;
        }
    }

    // Update is called once per frame
    void Update () {

        //Debug.Log(recovery);

        
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

        if(movable)
            Cc.Move(moveDirection * Time.deltaTime);

        // ***
        // Coups d'épée
        // ***
        if (Input.GetButtonDown("Fire2") && cooldown <= 5 && movable && actionCooldown <=0)
        {
            cooldown = time;
            E.SetActive(true);
            C.GetComponent<PlayerOrientation>().SetOrientable(false);
            Movable=false;
            actionCooldown = 30;

        }


        if (cooldown > 0)
        {
            cooldown--;
            if (cooldown == 0)
            {
                Movable=true;
                C.GetComponent<PlayerOrientation>().SetOrientable(true);
            }
            else if (cooldown <= 5)
            {
                E.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            TogglePauseGame();
            //GameObject.Find("Inventory").GetComponent<UI_Inventory>().DisplayInventory();
        }
        //Debug.Log(cooldown);

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
    }

    public bool Movable { get;set; }

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
        if (Input.GetButtonDown("Fire3")
            && (other.tag == "Ennemi" && cooldown <= 5))
            {
                Debug.Log(other);
                other.GetComponent<FoeBehaviour>().Dammages(1);
            }
        
    }

    public static int Level { get; set; }
    public static int MaxPv { get; set; }
    public static int Pv { get; set; }
    public static int MaxPm { get; set; }
    public static int Pm { get; set; }



    /* private void OnTriggerStay(Collider other){
        if (other.tag = "Chest") {
            if (Input.GetButtonDown("Fire3")){
            opened = true;
            GameObject.Find("Character").transform.LookAt(this.transform);
            this.GetComponent<Collider>().enabled = false;
            this.transform.localEulerAngles = new Vector3(100, 0, 0);
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            StartCoroutine(Opening());
            }
        }
       
    }*/




}
