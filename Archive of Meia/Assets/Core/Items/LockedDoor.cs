using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LockedDoor : MonoBehaviour
{
    private bool opened;
    private bool openable;
    private bool canOpen;

    // Start is called before the first frame update
    void Start()
    {
        openable = false;
        canOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && openable && !opened && canOpen)
        {
            opened = true;
            GameObject.Find("Player").GetComponent<InventoryCC>().AddKeys(-1);

            GameObject.Find("Character").transform.LookAt(transform);
            GetComponent<Collider>().enabled = false;

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            StartCoroutine(Opening());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            canOpen = true;
            openable = other.GetComponent<InventoryCC>().Key != 0;
            GameObject.Find(openable ? "I_Action" : "I_NAction").GetComponent<Image>().color = Color.white;
            GameObject.Find("T_Action").GetComponent<Text>().text = "Ouvrir";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("I_NAction").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
        canOpen = false;
        }
    }


    IEnumerator Opening()
    {
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=false;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(false);

        float i = -0.05f;

        do
        {
            transform.position += new Vector3(0, i, 0);

            yield return new WaitForEndOfFrame();

        } while (transform.position.y > -2.9f);
        
        //yield return new WaitForSeconds(1);

        yield return new WaitForEndOfFrame();
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=true;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(true);

    }
}
