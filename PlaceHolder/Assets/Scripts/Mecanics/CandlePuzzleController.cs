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
        foreach (Transform child in transform)
        {
            CandleForPuzzle candle = child.GetComponent<CandleForPuzzle>();
            if (candle != null)
            {
                candles.Add(candle);
            }
        }   
        correctOrder = new List<int>() { 0, 1, 2 };
        if (correctOrder.Count != candles.Count)
        {
            Debug.Log("Количество элементов в correctOrder и candles должно совпадать!");
        }
    }

    public void LightCandle(int candleIndex)
    {
        if (candleIndex == correctOrder[currentIndex])
        {
            candles[candleIndex].LitCandle();
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
