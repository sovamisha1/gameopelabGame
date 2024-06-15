using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staf : Interactable
{
    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public override void Interact()
    {
        inventory.AddItem("Посох Мага");
        Destroy(gameObject);
    }

    public override bool IsImportant()
    {
        return true;
    }
}
