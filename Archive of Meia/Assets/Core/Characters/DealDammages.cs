using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDammages : MonoBehaviour
{
    private PlayerCC PCC;

    private void Start()
    {
        PCC = GetComponentInParent<PlayerCC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnnemiHit")
        {
            other.transform.parent.GetComponent<FoeBehaviour>().Dammages(1);
            Debug.Log("Hit");
        }

        if (other.tag == "Destructible")
        {
            other.GetComponent<Destructible>().Destruction();
        }
    }
}
