using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoeBehaviour : MonoBehaviour
{
    public int pv;

    private bool playerDetected;
    private bool movable;
    public  bool stunned;
    private bool lootDispensed;

    //private float gravity = 10f;
    private int timer = 100;

    private Transform Player;

    [SerializeField]
    private List<Transform> Loot;

    private CharacterController Cc;
    private Animator An;
    private NavMeshAgent nav;

    private Vector3 Origine;
    private Vector3 moveDirection = Vector3.zero;

    public bool Movable { get => movable; set => movable = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (pv <= 0)
        {
            pv = 1;
        }

        movable = true;
        stunned = false;

        Origine = new Vector3(this.transform.position.x, transform.GetChild(0).transform.position.y, this.transform.position.z);

        Cc = this.GetComponent<CharacterController>();
        An = this.transform.GetChild(0).GetComponent<Animator>();
        Player = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();  
        lootDispensed=false;
    }


    void FixedUpdate (){

        /*if (Cc.isGrounded)
        {*/

            if (playerDetected && stunned==false)
            {

                An.SetBool("Attack", false);
                if (movable)
                {
                    nav.destination = Player.transform.position;
                    this.transform.GetChild(0).transform.LookAt(new Vector3(Player.position.x, transform.GetChild(0).transform.position.y, Player.position.z));
                    this.transform.GetChild(0).transform.rotation = this.transform.GetChild(0).transform.rotation * Quaternion.Euler(-90, 0, 0);
                    //this.transform.GetChild(0).transform.rotation = Quaternion.Euler(-90, this.transform.GetChild(0).transform.rotation.y, this.transform.GetChild(0).transform.rotation.z);
                    //Debug.Log(Player.position - new Vector3(0, -1000, 0));

                    An.SetBool("Walk", true);

                    float distance = Vector3.Distance(Player.transform.position, transform.position);

                }

                if (Vector3.Distance(Player.transform.position, transform.position) <= transform.GetChild(0).transform.localScale.x + 1)
                {
                    Movable = false;
                    An.SetBool("Attack", true);
                    StartCoroutine(CoolDown());
                }
            }
            else
            {

                if (Mathf.Abs(Origine.x - this.transform.position.x) > 1 || Mathf.Abs(Origine.z - this.transform.position.z) > 1)
                {
                    nav.destination = Origine;
                    this.transform.GetChild(0).transform.LookAt(new Vector3(Origine.x, transform.GetChild(0).transform.position.y, Origine.z));
                    this.transform.GetChild(0).transform.rotation = this.transform.GetChild(0).transform.rotation * Quaternion.Euler(-90, 0, 0);
                    //this.transform.GetChild(0).transform.rotation = this.transform.GetChild(0).transform.rotation * Quaternion.Euler(-90, 0, 0);
                    //this.transform.GetChild(0).transform.rotation = Quaternion.Euler(-90, this.transform.GetChild(0).transform.rotation.y, this.transform.GetChild(0).transform.rotation.z);
                    An.SetBool("Walk", true);
                }
                else
                {
                    An.SetBool("Walk", false);
                }

            }
        //}

        if (stunned){

            if(timer>0){
                timer--;
                //Debug.Log(timer);
            }else {
                stunned=false;
                timer = 100;
            }
        }




    }
    void Update()
    {
        
            //moveDirection.y = (-gravity * Time.deltaTime);  
            Cc.Move(moveDirection);
        


        if(this.transform.position.y < -5)
        {
            AutoDestruction();
        }
    }

    public void Dammages(int p)
    {
        pv--;

        //Cc.Move(transform.GetChild(0).transform.rotation * Vector3.up * 1.5f);

        if (pv <= 0)
        {
            if(lootDispensed==false){
                lootDispensed=true;
                loot();
                StartCoroutine(CoolDown());
                //Debug.Log(lootDispensed);
            }
            
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerDetected = false;
        }
    }

    private void loot()
    {
        if (Loot == null || Loot.Count == 0){
           return;
        }

        int i = (int)Random.Range(0, Loot.Count);

        if (Loot[i] == null){
            return;
        }
            Instantiate(Loot[i], new Vector3(transform.position.x, 0, transform.position.z), Loot[i].transform.rotation);
    }

    private void AutoDestruction()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator CoolDown()
    {
        
        yield return new WaitForSeconds(1.25f);
        Movable = true;
        An.SetBool("Attack", false);
    }
     private IEnumerator LootCoolDown()
    {
        
        yield return new WaitForSeconds(0.25f);
        lootDispensed=false;
    }
}
