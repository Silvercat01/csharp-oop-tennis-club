namespace nagybeadando_teniszpalya
{
    public interface IPricingStrategy
    {
        double CalculatePrice(double basePrice);
    }

    public class IgazoltPricingStrategy : IPricingStrategy
    {
        public double CalculatePrice(double basePrice) => basePrice * 0.6;
    }

    public class DiakPricingStrategy : IPricingStrategy
    {
        public double CalculatePrice(double basePrice) => basePrice * 0.8;
    }

    public class NyugdijasPricingStrategy : IPricingStrategy
    {
        public double CalculatePrice(double basePrice) => basePrice * 0.7;
    }

    public class AltalanosPricingStrategy : IPricingStrategy
    {
        public double CalculatePrice(double basePrice) => basePrice;
    }
}