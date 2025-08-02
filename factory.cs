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
