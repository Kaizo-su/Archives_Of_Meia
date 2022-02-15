using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    public float waitingTime;


    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOutFunction(0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeOutFunction(float p)
    {
        GameObject.Find("Player").GetComponent<PlayerCC>().Movable = false;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(false);

        float i = 1;

        yield return new WaitForSeconds(1);

        do
        {
            this.GetComponent<Image>().color = new Color(0, 0, 0, i);
            i -= 0.075f;
            yield return new WaitForSeconds(1 / 50f);
        } while (i > 0);

        yield return new WaitForSeconds(p / 3);
        this.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

        GameObject.Find("Player").GetComponent<PlayerCC>().Movable = true;
        GameObject.Find("Character").GetComponent<PlayerOrientation>().SetOrientable(true);
    }

}
