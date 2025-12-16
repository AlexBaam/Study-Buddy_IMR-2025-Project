using UnityEngine;

public class CandlesManager : MonoBehaviour
{
    public static CandlesManager Instance;

    public CandleController[] candles;
    private bool allLit = false;

    void Awake()
    {
        Instance = this;
    }

    public void ToggleAllCandles()
    {
        allLit = !allLit;

        foreach (var candle in candles)
        {
            if (allLit)
                candle.TurnOn();
            else
                candle.TurnOff();
        }
    }
}
