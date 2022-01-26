using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BootingScreen : MonoBehaviour
{
    private Image I;

    private GameObject Lang;

    void Awake()
    {
        Lang = GameObject.Find("Lang");
        Lang.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        I = GameObject.Find("Logo").GetComponent<Image>();
        StartCoroutine(Booting());
    }

    IEnumerator Booting()
    {
        float i = 0;

        yield return new WaitForSeconds(0.5f);

        do {
            I.color = new Color(1, 1, 1, i);
            i += 0.05f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i < 1);
        
        I.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(1f);

        i = 1;

        do
        {
            I.color = new Color(1, 1, 1, i);
            i -= 0.05f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i > 0);

        I.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(1f);

        Lang.SetActive(true);

        //SceneManager.LoadScene(1);
    }
}
