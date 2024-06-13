using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourch : Interactable
{
    bool isLit = false;

    public override void Interact()
    {
        if (isLit)
            Extinguish();
        else
            LitTourch();
    }

    private void LitTourch()
    {
        Debug.Log("Факел Зажжен");
        isLit = true;
    }

    private void Extinguish()
    {
        Debug.Log("Факел Потушен");
        isLit = false;
    }
}
