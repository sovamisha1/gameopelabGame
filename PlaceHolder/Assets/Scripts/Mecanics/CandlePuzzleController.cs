using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlePuzzleController : MonoBehaviour
{
    public List<CandleForPuzzle> candles;
    public List<int> correctOrder;
    private int currentIndex = 0;

    void Start()
    {
        correctOrder = new List<int>() { 1, 2, 3 };
        if (correctOrder.Count != candles.Count)
        {
            Debug.Log("Количество элементов в correctOrder и candles должно совпадать!");
        }
    }

    public void LightCandle(int torchIndex)
    {
        if (torchIndex == correctOrder[currentIndex])
        {
            candles[torchIndex].LitCandle();
            currentIndex++;
            Debug.Log("Правильно");

            if (currentIndex >= correctOrder.Count)
            {
                Debug.Log("Головоломка решена!");
            }
        }
        else
        {
            Debug.Log("АНЛАКИ");
            ExtinguishAllcandles();
            currentIndex = 0;
        }
    }

    private void ExtinguishAllcandles()
    {
        foreach (var torch in candles)
        {
            torch.Extinguish();
        }
    }
}
