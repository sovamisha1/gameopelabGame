using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void AddItem(string item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += 1;
        }
        else
        {
            inventory[item] = 1;
        }
    }

    public bool DoesContainItem(string item)
    {
        return inventory.ContainsKey(item);
    }

    public bool DoesContainItem(string item, int howMany)
    {
        return inventory.TryGetValue(item, out int count) && count == howMany;
    }

    public bool RemoveItem(string item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= 1;

            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }

            return true;
        }

        return false;
    }

    public bool RemoveItem(string item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            if (inventory[item] >= quantity)
            {
                inventory[item] -= quantity;

                if (inventory[item] <= 0)
                {
                    inventory.Remove(item);
                }

                return true;
            }
        }

        return false;
    }
}
