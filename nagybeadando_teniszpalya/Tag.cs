using System;

namespace nagybeadando_teniszpalya
{
    public class Tag
    {
        public enum Kedvezmeny
        {
            Igazolt,
            Diak,
            Nyugdijas,
            Altalanos
        }

        public string nev { get; private set; }
        public Kedvezmeny kedvezmeny { get; private set; }
        private IPricingStrategy _pricingStrategy;

        public Tag(string nev, Kedvezmeny kedvezmeny, IPricingStrategy pricingStrategy)
        {
            this.nev = nev ?? throw new ArgumentNullException(nameof(nev));
            this.kedvezmeny = kedvezmeny;
            _pricingStrategy = pricingStrategy ?? throw new ArgumentNullException(nameof(pricingStrategy));
        }

        public double ApplyDiscount(double basePrice)
        {
            return _pricingStrategy.CalculatePrice(basePrice);
        }
    }
}