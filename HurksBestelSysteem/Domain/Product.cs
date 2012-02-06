namespace HurksBestelSysteem.Domain
{
    public class Product
    {
        public string productName;
        public int productCode;
        public string description;
        public decimal price;
        public enum PriceType { UNIT=1, WEIGHT=2 }
        public PriceType priceType;

        public Product(string productName, int productcode, string description, decimal price, PriceType priceType)
        {
            this.productName = productName;
            this.productCode = productcode;
            this.description = description;
            this.price = price;
            this.priceType = priceType;
        }

        public override string ToString()
        {
            return productName + " (" + productCode + ") " + price.ToString();
        }
    }
}
