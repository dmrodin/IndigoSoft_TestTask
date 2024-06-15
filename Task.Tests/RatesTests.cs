namespace TestTask.Tests;

// Идея теста состоит в том, что мы гененрируем Count изменений котировок, где каждая котировка имеет вид Bid = i и Ask = i + 1
// Нам необходимо убедится, что не возникает случая когда Bid и Ask отличаются больше чем на 1, иначе это означает что полученные данные не корректные
// Для удобства тестирования, логика создания потоков и запуск генерации котировок и их чтение вынесены в класс TestHelper, который принимает интерфейс IRateStorage
// и количество итераций Count

public class RatesTests
{
    private int Count = 1000000;

    // Запускаем тест для изначального класса RatesStorage
    [Fact]
    public void TestLockless()
    {
        RatesStorage storage = new RatesStorage();

        TestHelper helper = new(storage, Count);
        helper.Start();

        Assert.True(helper.Success, "Data not as expected");
    }

    // Запускаем для класса в котором реализованы блокировки доступа к ресурсу
    [Fact]
    public void TestWithLock()
    {
        RatesStorageWithlLock storage = new RatesStorageWithlLock();

        TestHelper helper = new(storage, Count);
        helper.Start();

        Assert.True(helper.Success, "Data not as expected");
    }
   
}