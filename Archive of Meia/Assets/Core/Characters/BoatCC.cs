using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCC : MonoBehaviour {

    public float speedWalk = 10f;

    private Vector3 moveDirection = Vector3.zero;

    CharacterController Cc;

    // Use this for initialization
    void Start ()
    {
        Cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection *= speedWalk;
            
        moveDirection = transform.TransformDirection(moveDirection);
        
        Cc.Move(moveDirection * Time.deltaTime);
	}
}
