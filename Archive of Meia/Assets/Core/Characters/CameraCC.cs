using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCC : MonoBehaviour
{
    public int latence;

    private Transform target;
    public bool worldMapType;
    public bool interiorType;
    public bool ExteriorType;
    private Vector3 WorldMapView;
    private Vector3 InteriorView;
    private Vector3 ExteriorView;
    private Vector3 distance;
    private Vector3 positionCible;
    public bool teleport;
    public bool cameraFixe;

    // Use this for initialization
    void Start()
    {

        WorldMapView = new Vector3(0, 30, -9);
        InteriorView = new Vector3(0, 10, -6);
        ExteriorView = new Vector3(0, 7, -10);

        target = GameObject.Find("Player").GetComponent<Transform>();
        if (latence < 0)
            latence = 0;

        teleport = false;

        cameraFixe = worldMapType;

        ChangeViewType();
    }

    // Update is called once per frame
    void Update()
    {
        //this.GetComponent<Transform>().position = target.position + distance;
        if (!teleport)
        {
            if (cameraFixe)
            {
                this.GetComponent<Transform>().position = target.position + distance;
            }
            else
            {
                Lerp();
            }
            
        }
        //transform.LookAt(target);

        //Debug.Log(this.positionCible);
    }

    public Vector3 GetDistance()
    {
        return distance;
    }
    
    public void SetTeleport(bool p){
        teleport = p;
    }

    public void ResetCameraPosition()
    {
        ChangeViewType();
        positionCible = target.position + distance;
        this.GetComponent<Transform>().position = positionCible;
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

    public void changeViewTypeTo(byte type)
    {
        worldMapType = false;
        interiorType = false;
        ExteriorType = false;

        switch (type)
        {
            case 1:
                worldMapType = true;
                break;
            case 2:
                interiorType = true;
                break;
            case 3:
                ExteriorType = true;
                break;
            default:
                break;
        }

        ChangeViewType();
    }

    private void ChangeViewType()
    {
        if (worldMapType)
        {
            distance = WorldMapView;
            interiorType = false;
            ExteriorType = false;

            transform.eulerAngles = new Vector3(70, 0, 0);

        }
        else if (interiorType)
        {
            distance = InteriorView;
            worldMapType = false;
            ExteriorType = false;

            transform.eulerAngles = new Vector3(55, 0, 0);

        }
        else if (ExteriorType)
        {
            distance = ExteriorView;
            worldMapType = false;
            interiorType = false;

            transform.eulerAngles = new Vector3(30, 0, 0);
        }
    }

    /*public void ResetCamPosition()
    {

        if (worldMapType)
        {
            this.GetComponent<Transform>().position = target.position + WorldMapView;
            transform.eulerAngles = new Vector3(70, 0, 0);

        }
        else if (interiorType)
        {
            this.GetComponent<Transform>().position = target.position + InteriorView;
            transform.eulerAngles = new Vector3(55, 0, 0);

        }
        else if (ExteriorType)
        {
            this.GetComponent<Transform>().position = target.position + ExteriorView;
            transform.eulerAngles = new Vector3(30, 0, 0);
        }
    }*/
}
