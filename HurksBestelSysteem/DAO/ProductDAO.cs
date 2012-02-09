using System;
using System.Collections.Generic;
using HurksBestelSysteem.Domain;

namespace HurksBestelSysteem.DAO
{
    public interface ProductDAO
    {
        bool AddProduct(Product product);
        bool GetProductByCode(int productCode, out Product product);
        bool GetProductsByName(string productName, out Product[] products);
        bool RemoveProduct(Product p);
        bool GetProductsByCategory(ProductCategory[] categories, out Product[] products);
    }
}
