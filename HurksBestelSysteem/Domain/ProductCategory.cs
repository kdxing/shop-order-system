using System.Collections.Generic;
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

    public class CategoryComparator : IEqualityComparer<ProductCategory>
    {
        public bool Equals(ProductCategory c1, ProductCategory c2)
        {
            if (c1.internalID.Equals(-1) || c2.internalID.Equals(-1))
            {
                //at least one of the categories is not stored in the database, so doesn't have an ID
                //compare by name then
                if (c1.name.Equals(c2.name))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (c1.internalID.Equals(c2.internalID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(ProductCategory category)
        {
            int combined = category.name.GetHashCode() + category.internalID.GetHashCode();
            return combined.GetHashCode();
        }
    }
}
