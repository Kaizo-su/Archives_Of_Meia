using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" )
        {
            GameObject.Find("Player").GetComponent<InventoryCC>().Money=value;
            Destroy(gameObject);
        }
    }
}
