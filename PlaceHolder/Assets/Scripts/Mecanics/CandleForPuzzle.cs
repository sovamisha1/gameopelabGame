using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleForPuzzle : Interactable
{
    bool isLit = false;

    public override void Interact()
    {
        CandlePuzzleController controller = FindObjectOfType<CandlePuzzleController>();
        controller.LightCandle(this.transform.GetSiblingIndex());
    }

    public void LitCandle()
    {
        Debug.Log("Свечка Зажженна");
        isLit = true;
    }

    public void Extinguish()
    {
        Debug.Log("Свечка Потушена");
        isLit = false;
    }
}
