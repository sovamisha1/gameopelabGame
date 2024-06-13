using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : Interactable
{
    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public override void Interact()
    {
        inventory.AddItem("Зелье Лечения");
        Destroy(gameObject);
    }

    public override bool IsImportant()
    {
        return true;
    }
}
