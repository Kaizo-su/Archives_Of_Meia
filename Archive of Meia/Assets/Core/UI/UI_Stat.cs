using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    private PlayerCC Pcc;
    private InventoryCC Icc;

    private Text Lv;  //Objet de Ui de pause:     Niveau.
    private Text Pv;  //Objet de Ui de pause:     Santé.
    private Text Pm;  //Objet de Ui de pause:     Magie.
    private Text Gk;  //Objet de Ui de pause:     Gold Key.
    private Text Rk;  //Objet de Ui de pause:     Regular Key.
    private Text Mn;  //Objet de Ui de pause:     Monney.

    // Awake is called before Start
    void Awake()
    {
        Pcc = GameObject.Find("Player").GetComponent<PlayerCC>();
        Icc = GameObject.Find("Player").GetComponent<InventoryCC>();

        Lv = transform.GetChild(3).gameObject.GetComponent<Text>();
        Pv = transform.GetChild(4).gameObject.GetComponent<Text>();
        Pm = transform.GetChild(5).gameObject.GetComponent<Text>();
        Gk = transform.GetChild(9).gameObject.GetComponent<Text>();
        Rk = transform.GetChild(10).gameObject.GetComponent<Text>();
        Mn = transform.GetChild(11).gameObject.GetComponent<Text>();

        Actualisation();
    }

    // Update is called once per frame
    void Update() { }

    public void Actualisation()
    {
        Lv.text = PlayerCC.Level.ToString();
        Pv.text = PlayerCC.Pv + "/" + PlayerCC.MaxPv;
        Pm.text = PlayerCC.Pm + "/" + PlayerCC.MaxPm;

        /*Gk.text = (Icc.GoldKey ? "1" : "0");
        Rk.text = Icc.Key.ToString();
        Mn.text = Icc.Money.ToString();*/
    }
}
