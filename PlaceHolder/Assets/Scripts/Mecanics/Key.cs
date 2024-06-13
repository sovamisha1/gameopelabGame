using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoorColntroller;

public class Key : Interactable
{
    public KeyType keyType;
    private Inventory inventory;

    private Dictionary<KeyType, string> dict = new Dictionary<KeyType, string>
    {
        { KeyType.None, "none" },
        { KeyType.Red, "Красный ключь" },
        { KeyType.Blue, "Синий ключь" }
    };

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public override void Interact()
    {
        if (keyType != KeyType.None)
        {
            string keyName;
            if (dict.TryGetValue(keyType, out keyName))
            {
                inventory.AddItem(keyName);
            }
            Destroy(gameObject);
        }
    }
}
