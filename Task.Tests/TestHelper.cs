using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask
{
    public class TestHelper
    {
        public bool Success { get; set; }

        private IRateStorage storage; // Чтобы сравнить две реализации RatesStorage, передаем в хелпер интерфейс IRateStorage
        private int count;

        public TestHelper(IRateStorage storage, int count)
        {
            this.storage = storage;
            this.count = count;

            Success = true;
        }

        public void Start()
        {
            Thread tUpdate = new Thread(UpdateRate);
            Thread tRead = new Thread(ReadRate);

            tUpdate.Start();
            tRead.Start();

            tUpdate.Join();
            tRead.Join();
        }     

        void UpdateRate()
        {
            for (int i = 1; i < count; i++)
            {
                storage.UpdateRate(new NativeRate
                {
                    Time = DateTime.Now,
                    Ask = i + 1,
                    Bid = i,
                    Symbol = "EURUSD"
                });
            }
        }

        void ReadRate()
        {
            for (int i = 1; i < count; i++)
            {
                var rate = storage.GetRate("EURUSD");

                if (rate != null && (rate.Bid != 0 && rate.Ask != 0))
                {
                    if (rate.Bid + 1 != rate.Ask)
                    {
                        Success = false;
                    }
                }                
            }
        }
    }
}
