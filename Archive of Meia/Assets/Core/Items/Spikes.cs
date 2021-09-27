using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private bool moveDown;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Down());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            other.GetComponent<PlayerCC>().Hit(5);
        }
    }

    IEnumerator Up()
    {
        float i = 0.0005f;

        do
        {
            transform.localPosition += new Vector3(0, 0, i);

            yield return new WaitForEndOfFrame();

        } while (transform.localPosition.z < 0);

        yield return new WaitForSeconds(3);

        StartCoroutine(Down());
    }
    
    IEnumerator Down()
    {
        float i = -0.0005f;

        do
        {
            transform.localPosition += new Vector3(0, 0, i);

            yield return new WaitForEndOfFrame();

        } while (transform.localPosition.z > -0.01f);

        yield return new WaitForSeconds(3);

        StartCoroutine(Up());
    }
}
