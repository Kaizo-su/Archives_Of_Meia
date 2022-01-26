using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LockedBossDoor : MonoBehaviour
{
    private bool opened;

    private bool openable;

    // Start is called before the first frame update
    void Start()
    {
        openable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && openable && !opened)
        {
            opened = true;
            //GameObject.Find("Player").GetComponent<InventoryCC>().GoldKey = false;

            //GameObject.Find("Character").transform.LookAt(this.transform);
            this.GetComponent<Collider>().enabled = false;

            GameObject.Find("I_Action").GetComponent<Image>().color = Color.clear;
            GameObject.Find("T_Action").GetComponent<Text>().text = "";
            StartCoroutine(Opening());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            //openable = !other.GetComponent<InventoryCC>().GoldKey;
            GameObject.Find(openable?"I_Action":"I_NAction").GetComponent<Image>().color = Color.white;
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    IEnumerator Opening()
    {
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=false;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(false);

        int i = 0;

        float i1 = -0.5f;
        float i2 = 0.5f;

        do
        {
            transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, i1 * i);
            transform.GetChild(1).transform.localRotation = Quaternion.Euler(0, 0, i2 * i);
            i++;
            yield return new WaitForEndOfFrame();

        } while (i2 * i < 90);

        //yield return new WaitForSeconds(1);

        yield return new WaitForEndOfFrame();
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable=true;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(true);

    }
}
