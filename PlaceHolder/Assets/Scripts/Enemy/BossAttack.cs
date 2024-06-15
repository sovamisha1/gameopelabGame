using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBodyCollider"))
        {
            playerController.ReceivingDamage(25f);
            Destroy(gameObject);
        }
    }
}
