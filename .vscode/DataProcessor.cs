public class DataProcessor
{
    private readonly string _processorId;

    public DataProcessor(int id)
    {
        _processorId = $"Processor-{id}";
        Console.WriteLine($"{_processorId} created and subscribed to the event.");

        // مشترک شدن در رویداد استاتیک
        EventManager.DataProcessed += OnDataProcessed;
    }

    private void OnDataProcessed(object sender, string data)
    {
        Console.WriteLine($"{_processorId} received data: {data}");
    }

    // از Finalizer (Destructor) برای نمایش اینکه آیا GC شیء را پاک می‌کند یا نه، استفاده می‌کنیم
    ~DataProcessor()
    {
        Console.WriteLine($"!!!!!!!! {_processorId} was finalized (garbage collected). !!!!!!!!");
    }
}
