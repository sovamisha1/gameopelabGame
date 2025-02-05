using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Interactable
{
    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public override void Interact()
    {
        inventory.AddItem("Кроваво-красный кристалл");
        Destroy(gameObject);
    }
}
