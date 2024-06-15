using System.Collections.Concurrent;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask;

public class RatesStorage : IRateStorage
{
    private Dictionary<string, Rate> rates = new();

    public void UpdateRate(NativeRate newRate)
    {
        if (rates.ContainsKey(newRate.Symbol) == false)
        {
            rates.Add(newRate.Symbol, new Rate());
        }

        var oldRate = rates[newRate.Symbol];

        oldRate.Time = newRate.Time;
        oldRate.Bid = newRate.Bid;
        oldRate.Ask = newRate.Ask;
    }

    public Rate GetRate(string symbol)
    {
        if (rates.ContainsKey(symbol) == false)
        {
            return null;
        }

        return rates[symbol];
    }
}