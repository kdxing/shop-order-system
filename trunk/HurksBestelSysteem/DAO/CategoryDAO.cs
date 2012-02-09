using HurksBestelSysteem.Domain;

namespace HurksBestelSysteem.DAO
{
    public interface CategoryDAO
    {
        bool GetAllCategories(out ProductCategory[] categories);
        bool RemoveCategory(ProductCategory category);
        bool AddCategory(ProductCategory category);
    }
}
