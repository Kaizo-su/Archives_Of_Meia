using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private ParticleSystem P;
    private Light L;
        
    // Start is called before the first frame update
    void Start()
    {
        P = this.transform.GetChild(0).GetComponent<ParticleSystem>();
        L = this.transform.GetChild(1).GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
