using System.ComponentModel;

namespace TestTask.Models;

public class Rate
{
    public DateTime Time { get; set; }
    public string Symbol { get; set; }
    public double Bid { get; set; }
    public double Ask { get; set; }

}