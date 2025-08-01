// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Threading.Tasks;

Console.WriteLine("Starting simulation...");








Console.ReadLine();

//===================== Strategy ==================
public class Order
{
    public decimal TotalAmount { get; set; }
}
public interface Idiscount
{

    public decimal Calculator(Order order);
}
public class Strategy : Idiscount
{
    Idiscount _discount;


    public Strategy(Idiscount idiscount)
     {

        _discount = idiscount;
      }

  public decimal Calculator(Order order)
  {
    return _discount.Calculator(order);
  }

  public void Set(Idiscount idiscount)
  {
    _discount = idiscount;
  }
}
public class Guest : Idiscount
{
    public decimal Calculator(Order order)
    {
        Console.WriteLine("Applying no discount for Guest.");
        return 0;
    }
}
public enum CustomerType
{
    Guest,
    Regular,
    Premium
}
//===================== factory ===============================
public interface IDiscountStrategy { decimal CalculateDiscount(Order order); }
public class GuestDiscountStrategy : IDiscountStrategy
{
    public decimal CalculateDiscount(Order order)
    {
        throw new NotImplementedException();
    }
}
public class DiscountStrategyFactory
{
    public IDiscountStrategy CreateStrategy(CustomerType type)
    {
        switch (type)
        {
            case CustomerType.Guest:
                return new GuestDiscountStrategy();
            default:
                throw new ArgumentOutOfRangeException(nameof(type), "Customer type not supported.");
        }
    }
}
public class DiscountContext
{
    private IDiscountStrategy _strategy;

    public void SetStrategy(IDiscountStrategy strategy)
    {
        _strategy = strategy;
    }
    public decimal ApplyDiscount(Order order)
    {
        if (_strategy == null)
        {
            throw new InvalidOperationException("Strategy not set.");
        }
        return _strategy.CalculateDiscount(order);
    }
}
//===================== command pattern  =======================
// Command
public interface ICommand
{
    void Execute();
    void Undo();
}
public class Light
{
    public void On() => Console.WriteLine("Light is ON");
    public void Off() => Console.WriteLine("Light is OFF");
}
public class LightOnCommand : ICommand
{
    private readonly Light _light;

    public LightOnCommand(Light light)
    {
        _light = light;
    }

    public void Execute()
    {
        _light.On();
    }

    public void Undo()
    {
        _light.Off();
    }
}
public class RemoteControl
{
    private ICommand[] _commands = new ICommand[2];
    private ICommand _lastCommand;

    public RemoteControl()
    {
        ICommand noCommand = new NoCommand();
        for (int i = 0; i < _commands.Length; i++)
        {
            _commands[i] = noCommand;
        }
        _lastCommand = noCommand;
    }

    public void SetCommand(int slot, ICommand command)
    {
        _commands[slot] = command;
    }

    public void PressButton(int slot)
    {
        _commands[slot].Execute();
        _lastCommand = _commands[slot];
    }

    public void PressUndoButton()
    {
        Console.Write("Undo pressed: ");
        _lastCommand.Undo();
    }   
}
public class NoCommand : ICommand
{
    public void Execute() { }
    public void Undo() { }
}
//===================== SpanToSubString  =======================
public static class SpanToSubString
{
    public static List<string> GetFirstAndLastName_Inefficient(string fullName)
    {
        ReadOnlySpan<char> fullNameSpan = fullName.AsSpan();
        int spaceIndex = fullNameSpan.IndexOf(' ');
        if (spaceIndex == -1)
        {
            return new List<string>() { fullName, string.Empty };
        }
        ReadOnlySpan<char> firstNameSpan = fullNameSpan.Slice(0, spaceIndex);
        ReadOnlySpan<char> lastNameSpan = fullNameSpan.Slice(spaceIndex + 1);
        return new List<string>() { firstNameSpan.ToString(), firstNameSpan.ToString() };
    }
}
//===================== memory leak on delegate event  =======================
public class DataProcessor : IDisposable
{
    private readonly string _processorId;

    public DataProcessor(int id)
    {
        _processorId = $"Processor-{id}";
        Console.WriteLine($"{_processorId} created and subscribed to the event.");
        EventManager.DataProcessed += OnDataProcessed;
    }

    private void OnDataProcessed(object sender, string data)
    {
        Console.WriteLine($"{_processorId} received data: {data}");
    }

    public void Dispose()
    {
        EventManager.DataProcessed -= OnDataProcessed;
        Console.WriteLine($"!!!!!!!! {_processorId} was finalized (garbage collected). !!!!!!!!");
    }
}

public static class EventManager
{
    public static event EventHandler<string> DataProcessed;

    public static void RaiseDataProcessed(string data)
    {
        DataProcessed?.Invoke(null, data);
    }
}

//==================== memory Leak singleton scoped DI ========================
public class MyScopedService : IDisposable
{
    private readonly Guid _id = Guid.NewGuid();
    public void Dispose() => Console.WriteLine($"Scoped service {_id} disposed.");
    public Guid GetId() => _id;
}
public class MyCorrectSingletonService
{
    private readonly IServiceProvider _serviceProvider;

    // به جای سرویس Scoped، IServiceProvider را تزریق می‌کنیم
    public MyCorrectSingletonService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void DoWorkInScope()
    {
        Console.WriteLine("Singleton is starting new work...");

        // using (var scope = _serviceProvider.CreateScope())
        // {
        //     var scopedService = scope.ServiceProvider.GetRequiredService<MyScopedService>();
        //     Console.WriteLine($"Singleton is using a NEW scoped service with ID: {scopedService.GetId()}");

        // }
    }
}

//================= bitArray instead of bool  ================
public class UserStatusTracker_Efficient
{

    private BitArray _userStatus;
    public UserStatusTracker_Efficient(int maxUsers)
    {
        _userStatus = new BitArray(maxUsers);
    }

    public void SetUserOnline(int userId)
    {
        if (userId >= _userStatus.Length)
        {
            _userStatus.Length = userId + 1;
        }
        _userStatus[userId] = true;
    }

    public bool IsUserOnline(int userId)
    {
        if (userId >= _userStatus.Length)
        {
            return false;
        }
        return _userStatus[userId];
    }
}
//================= lookup  ================
public class SaleRecord
{
    public string Region { get; set; }
    public decimal Amount { get; set; }
}
public class SalesReporter_Inefficient
{
    private readonly ILookup<string, SaleRecord> _allSales;

    public SalesReporter_Inefficient(List<SaleRecord> allSales)
    {
        _allSales = allSales.ToLookup(p => p.Region);
    }

    public decimal GetTotalSalesForRegion(string region)
    {
        return _allSales[region].Sum(c => c.Amount);
    }

    public int GetSalesCountForRegion(string region)
    {
        return _allSales[region].Count();
    }
}
//============== Dictionary ==================
public class Product
{
    public int Id { get; set; }
    public int LName { get; set; }
}
public class GetValFromDictionary
{
    public void Findproduct()
    {
        var allProducts = new List<Product>() { new Product { Id = 1, LName = 10 }, };
        var productIdsInStock = new List<int>() { 1, 2, 3, 4 };
        var productMap = allProducts.ToDictionary(p => p.Id);

        foreach (int id in productIdsInStock)
        {
            if (productMap.TryGetValue(id, out var product))
            {
                Console.WriteLine(product.LName);
            }
        }
    }
}
//======================== Sum In List To Target ========================
public class SumInListToTarget
{
    public void GetTarget()
    {
        int[] nums = [7, 5, 2, 11, 15, 4];
        var dic = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            var current = nums[i];
            var complited = 9 - current;
            if (dic.ContainsKey(complited))
            {
                var c = dic[complited];
            }
            if (!dic.ContainsKey(complited))
            {
                dic.Add(current, i);
            }
        }
    }
}
//======================== Duplicate List ========================
public class DuplicateInt
{
    public void RemoveDuplicate()
    {
        var listWithDuplicates = new List<int>();

        for (int i = 0; i < 10000; i++)
        {
            listWithDuplicates.Add(i);
            listWithDuplicates.Add(1);
        }
        var uniqueItems = new HashSet<int>(listWithDuplicates);
        var uniqueItems1 = listWithDuplicates.Distinct().ToList();
    }
}



       