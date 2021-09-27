using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PNJ_Default", menuName = "PNJ settings")]
public class PNJ : MonoBehaviour
{
    public Dialogue dialogue;

    [SerializeField]
    [TextArea(1, 15)]
    private string nameLeft;
    [SerializeField]
    [TextArea(1, 15)]
    private string nameRight;
    [SerializeField]
    //[TextArea(3, 15)]
    //private string[] dialogue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
