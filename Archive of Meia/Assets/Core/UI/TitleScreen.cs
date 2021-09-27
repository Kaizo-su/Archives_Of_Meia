using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    private Image I;

    private void Awake()
    {
        I = GameObject.Find("CanvasCache").GetComponent<Image>();
        I.color = Color.black;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Booting());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator Booting()
    {
        float i = 1;

        do
        {
            I.color = new Color(0, 0, 0, i);
            i -= 0.02f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i > 0);

        I.color = new Color(0, 0, 0, 0);

        yield return new WaitForSeconds(2f);
    }
}
