using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public int recover;
    private int pcPV;
    private int pcMaxPV;

    void Start ()
    {
    	/*pcPV = GameObject.Find("Player").GetComponent<PlayerCC>().PV;
    	pcMaxPV = GameObject.Find("Player").GetComponent<PlayerCC>().MaxPV;*/
    	pcPV = PlayerCC.GetPv();
    	pcMaxPV = PlayerCC.GetMaxPv();
    }

    private void OnTriggerStay(Collider other)
    {	

        if (other.name == "Player")
        {
        	pcPV = PlayerCC.GetPv();
    		pcMaxPV = PlayerCC.GetMaxPv();
    		if (pcPV < pcMaxPV){
    			GameObject.Find("Player").GetComponent<PlayerCC>().Heal(recover);
            	Destroy(this.gameObject);
    		}
            
        }

    }
}
