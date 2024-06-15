using TestTask.Models;

namespace TestTask.Interfaces
{
    public interface IRateStorage
    {
        public void UpdateRate(NativeRate newRate);
        public Rate? GetRate(string symbol);
    }
}
