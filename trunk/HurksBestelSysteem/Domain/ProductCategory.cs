namespace HurksBestelSysteem.Domain
{
    public sealed class ProductCategory
    {
        public string name;
        public string description;
        public int internalID; //id in database

        public ProductCategory(string name, string description)
        {
            this.name = name;
            this.description = description;
            this.internalID = -1;
        }

        public ProductCategory(string name, string description, int internalID)
        {
            this.name = name;
            this.description = description;
            this.internalID = internalID;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
