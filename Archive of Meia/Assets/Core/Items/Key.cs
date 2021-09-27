using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" )
        {
            GameObject.Find("Player").GetComponent<InventoryCC>().AddKeys(1);
            Destroy(this.gameObject);
        }
    }
}
