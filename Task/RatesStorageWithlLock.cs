using System.Collections.Concurrent;
using TestTask.Interfaces;
using TestTask.Models;

// В код ниже добавлены комментарии что изменил по сравнению с исходным классом RatesStorage

namespace TestTask
{
    public class RatesStorageWithlLock : IRateStorage 
    {
        private ConcurrentDictionary<string, Rate> _rates = new(); // Заменяем коллекцию Dictionary на ее конкурентную реализацию
        
        private ReaderWriterLockSlim _locker = new ReaderWriterLockSlim(); // Создаем объект класса ReaderWriterLockSlim через который будем управлять
                                                                           // блокировкой общего ресурса при чтении и обновлении 
        public Rate? GetRate(string symbol)
        {
            _locker.EnterReadLock();    
            try
            {
                if (_rates.ContainsKey(symbol) == false)
                {
                    return null;
                }

                // Создаем новый объект класса Rate, иначе если передадим ссылку на _rates[symbol] мы не можем быть уверенными, что пока вызывающий метод обработает данные,
                // значение Bid и Ask не примут другое значение
                Rate rate = new Rate 
                {
                    Symbol = symbol,
                    Time = _rates[symbol].Time,
                    Ask = _rates[symbol].Ask,
                    Bid = _rates[symbol].Bid
                };

                return rate;
            }
            finally
            {
                _locker.ExitReadLock(); 
            }

        }

        public void UpdateRate(NativeRate newRate)
        {
            _locker.EnterUpgradeableReadLock();
            try
            {
                if (_rates.ContainsKey(newRate.Symbol) == false)
                {
                    _rates.TryAdd(newRate.Symbol, new Rate());
                }                

                _locker.EnterWriteLock();
                try
                {
                    var oldRate = _rates[newRate.Symbol];

                    oldRate.Time = newRate.Time;
                    oldRate.Bid = newRate.Bid;
                    oldRate.Ask = newRate.Ask;
                }
                finally
                {
                    _locker.ExitWriteLock();
                }

            }
            finally
            {
                _locker.ExitUpgradeableReadLock();
            }
        }

        
    }
}
