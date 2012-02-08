namespace HurksBestelSysteem.Domain
{
    public class Product
    {
        public string productName;
        public int productCode;
        public int internalID; //id in database
        public string description;
        public decimal price;
        public enum PriceType { UNIT=1, WEIGHT=2 }
        public PriceType priceType;
        public ProductCategory[] categories;

        public Product(string productName, int productcode, string description, decimal price, PriceType priceType, ProductCategory[] categories)
        {
            this.productName = productName;
            this.productCode = productcode;
            this.description = description;
            this.price = price;
            this.priceType = priceType;
            this.categories = categories;

            this.internalID = -1;
        }

        public Product(string productName, int productcode, string description, decimal price, PriceType priceType, ProductCategory[] categories, int internalID)
        {
            this.productName = productName;
            this.productCode = productcode;
            this.description = description;
            this.price = price;
            this.priceType = priceType;
            this.categories = categories;

            this.internalID = internalID;
        }

        public override string ToString()
        {
            return productName + " (" + productCode + ") "  + description.ToString();
        }
    }
}
