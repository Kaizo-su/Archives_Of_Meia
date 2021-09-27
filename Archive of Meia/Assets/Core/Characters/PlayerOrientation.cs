using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour {

    float axe = 0.1f;
    //float heading = 0;

    private bool orientable = true;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis("Horizontal") > axe || Input.GetAxis("Horizontal") < -axe ||
            Input.GetAxis("Vertical") > axe || Input.GetAxis("Vertical") < -axe)
        {
            if (orientable)
            {
                //transform.rotation = Quaternion.Euler(-90, (Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg) + 90, 0);
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg, 0);
                //this.transform.LookAt(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            }
        }
    }

    public void SetOrientable(bool p)
    {
        orientable = p;
    }
}
