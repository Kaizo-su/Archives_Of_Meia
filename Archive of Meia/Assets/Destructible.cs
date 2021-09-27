using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Loot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Destruction()
    {
        loot();
        Destroy(this.gameObject);
    }

    private void loot()
    {
        if (Loot == null || Loot.Count == 0)
            return;

        int i = (int)Random.Range(0, Loot.Count);

        if (Loot[i] == null)
            return;

        Instantiate(Loot[i], new Vector3(transform.position.x, 0, transform.position.z), Loot[i].transform.rotation);
    }
}
