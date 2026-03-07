using ManagmentSystem.Models;
using System.Collections.Generic;

public interface IProductService
{
    void AddProduct(string name, string description, int quantity, decimal salePrice, decimal costPrice, int categoryId);
    bool UpdateProduct(int id, string name, string description, int quantity, decimal salePrice, decimal costPrice, int categoryId);
    bool DeleteProduct(int id);
    List<Product> GetAllProducts();
    List<Product> GetProductsByCategory(int categoryId);
    Product GetProductById(int id);
    List<Product> SearchProducts(string name = null, decimal? minPrice = null, decimal? maxPrice = null, int? categoryId = null);
    void SoftDeleteProductsByCategory(int categoryId); // لإلغاء المنتجات المرتبطة بتصنيف
}
