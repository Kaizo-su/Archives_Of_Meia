using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public abstract class Chest : MonoBehaviour
{
    private bool opened;
    private bool canOpen;

    private Transform couvercle;
    
    // Start is called before the first frame update
    void Start()
    {
        couvercle = this.transform.GetChild(0);

        if (opened)
        {
            this.GetComponent<Collider>().enabled = false;
            this.transform.localEulerAngles = new Vector3(100, 0, 0);
        }
        canOpen=false;
    }

    protected abstract void loot();

    void Update()
    {

        if (Input.GetButtonDown("Fire2") && canOpen == true && opened == false)
        {
            opened = true;

            //GameObject.Find("Character").transform.LookAt(this.transform);
            //this.transform.GetChild(0).GetComponent<Collider>().enabled = false;
            couvercle.transform.localEulerAngles = new Vector3(100, 0, 0);

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            StartCoroutine(Opening());
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.white;
            GameObject.Find("T_Action").GetComponent<Text>().text = "Ouvrir";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            canOpen = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            canOpen = true;
        }
    }


    protected IEnumerator Opening()
    {
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=false;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(false);

        float i = 0;

        do
        {
            couvercle.localEulerAngles = new Vector3(i, 0, 0);
            i += 2f;
            yield return new WaitForEndOfFrame();

        } while (couvercle.localEulerAngles.x <= 80);

        loot();

        //yield return new WaitForSeconds(1);

        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=true;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(true);

    }
}
