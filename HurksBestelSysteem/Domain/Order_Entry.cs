namespace HurksBestelSysteem.Domain
{
    public sealed class Order_Entry
    {
        public Order parentOrder;
        public Product product;
        public int quantity;
        public int internalID;

        public Order_Entry(Order parentOrder, Product product, int quantity)
        {
            this.parentOrder = parentOrder;
            this.product = product;
            this.quantity = quantity;
            this.internalID = -1;
        }

        public Order_Entry(Order parentOrder, Product product, int quantity, int internalID)
        {
            this.parentOrder = parentOrder;
            this.product = product;
            this.quantity = quantity;
            this.internalID = internalID;
        }

        public override string ToString()
        {
            return product.productName + " x " + quantity;
        }
    }
}
