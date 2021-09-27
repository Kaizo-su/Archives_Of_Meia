using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCC : MonoBehaviour
{
    public int latence;

    private Transform target;
    public Vector3 distance;
    private Vector3 positionCible;
    public static bool Teleport;

    // Use this for initialization
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        if (latence < 0)
            latence = 0;
            Teleport=false;
    }

    // Update is called once per frame
    void Update()
    {
        //this.GetComponent<Transform>().position = target.position + distance;
        if (Teleport){
                Tp();
            }else {
                Lerp();
            }
        
    }

    public Vector3 GetDistance()
    {
        return distance;
    }

    void Tp(){
        Teleport=false;
        this.GetComponent<Transform>().position = new Vector3(positionCible.x, 0, positionCible.z);
    }

    void Lerp()
    {
        positionCible = target.position + distance;

        if (this.GetComponent<Transform>().position.x > positionCible.x)
        {
            this.GetComponent<Transform>().position -= new Vector3(this.GetComponent<Transform>().position.x - positionCible.x, 0, 0) / latence;
        }
        else if (this.GetComponent<Transform>().position.x < positionCible.x)
        {
            this.GetComponent<Transform>().position += new Vector3(positionCible.x - this.GetComponent<Transform>().position.x, 0, 0) / latence;
        }

        if (this.GetComponent<Transform>().position.y > positionCible.y)
        {
            this.GetComponent<Transform>().position -= new Vector3(0, this.GetComponent<Transform>().position.y - positionCible.y, 0) / latence;
        }
        else if (this.GetComponent<Transform>().position.y < positionCible.y)
        {
            this.GetComponent<Transform>().position += new Vector3(0, positionCible.y - this.GetComponent<Transform>().position.y, 0) / latence;
        }

        if (this.GetComponent<Transform>().position.z > positionCible.z)
        {
            this.GetComponent<Transform>().position -= new Vector3(0, 0, this.GetComponent<Transform>().position.z - positionCible.z) / latence;
        }
        else if (this.GetComponent<Transform>().position.z < positionCible.z)
        {
            this.GetComponent<Transform>().position += new Vector3(0, 0, positionCible.z - this.GetComponent<Transform>().position.z) / latence;
        }

    }
}
