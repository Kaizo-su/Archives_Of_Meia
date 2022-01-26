using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCC : MonoBehaviour
{
    private int time;
    private int cooldown;

    private GameObject E;

    // Start is called before the first frame update
    void Start()
    {
        time = 15;
        cooldown = time;
        E = GameObject.Find("epee");
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(cooldown);

        if (cooldown <= 5)
        {
            cooldown = time;
            E.SetActive(true);
            this.GetComponent<PlayerCC>().Movable=false;

        }

        if (cooldown > 0)
        {
            cooldown--;

            if (cooldown == 0)
            {
                this.GetComponent<PlayerCC>().Movable=true;
            }
            else if (cooldown <= 5)
            {
                E.SetActive(false);
            }
        }*/
    }
}
