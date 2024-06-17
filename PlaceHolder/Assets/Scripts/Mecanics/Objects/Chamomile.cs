using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Interactable
{
    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public override void Interact()
    {
        inventory.AddItem("Ромашка");
        Destroy(gameObject);
    }
}
