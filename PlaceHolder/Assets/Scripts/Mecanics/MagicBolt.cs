using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBolt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            other.GetComponent<Boss>().TakeDamage(25f);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
            return;
    }
}
