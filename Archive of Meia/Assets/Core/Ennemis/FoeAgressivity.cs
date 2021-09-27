using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeAgressivity : MonoBehaviour
{
    private GameObject P;

    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == P.name)
        {
            P.GetComponent<PlayerCC>().Hit(5);
            transform.parent.GetComponent<FoeBehaviour>().stunned = true;
        }
    }
}
