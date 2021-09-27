using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Foes;

    private bool locked;
    private bool finish;

    private GameObject Walls;
    private GameObject Enemies;

    // Start is called before the first frame update
    void Start()
    {
        Walls = this.transform.GetChild(0).gameObject;
        Enemies = this.transform.GetChild(1).gameObject;

        Walls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
        {
            if (Enemies.transform.childCount == 0)
            {
                locked = false;
                finish = true;

                Walls.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !locked && !finish)
        {

            if (Foes == null || Foes.Count == 0)
                return;

            Walls.SetActive(true);

            GameObject k;
            int j = 0;

            foreach (GameObject i in Foes)
            {
                k = Instantiate(i, new Vector3(transform.position.x + (j % 2 == 0 ? -2 : 1) * j, transform.position.y + 1, transform.position.z), i.transform.rotation);
                k.gameObject.transform.SetParent(Enemies.transform);
                j++;
            }

            locked = true;
        }
    }
}
