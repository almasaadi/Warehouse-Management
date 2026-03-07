using System;
using System.Collections.Generic;
using System.Linq;
using ManagmentSystem.Data;
using ManagmentSystem.Models;

namespace ManagmentSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly JsonHelper<Product> _jsonHelper;
        private List<Product> _products;

        public ProductService()
        {
            _jsonHelper = new JsonHelper<Product>("products.json");
            _products = _jsonHelper.Load();
        }

        public void AddProduct(string name, string description, int quantity, decimal salePrice, decimal costPrice, int categoryId)
        {
            var newId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(new Product
            {
                Id = newId,
                Name = name?.Trim() ?? "",
                Description = description?.Trim() ?? "",
                Quantity = quantity,
                SalePrice = salePrice,
                CostPrice = costPrice,
                CategoryId = categoryId,
                IsDeleted = false
            });
            _jsonHelper.Save(_products);
        }

        public bool UpdateProduct(int id, string name, string description, int quantity, decimal salePrice, decimal costPrice, int categoryId)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (product == null) return false;

            product.Name = name?.Trim() ?? product.Name;
            product.Description = description?.Trim() ?? product.Description;
            product.Quantity = quantity;
            product.SalePrice = salePrice;
            product.CostPrice = costPrice;
            product.CategoryId = categoryId;

            _jsonHelper.Save(_products);
            return true;
        }

        public bool DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (product == null) return false;

            product.IsDeleted = true; // Soft Delete
            _jsonHelper.Save(_products);
            return true;
        }

        public List<Product> GetAllProducts() => _products.Where(p => !p.IsDeleted).ToList();
        public Product GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
        public List<Product> GetProductsByCategory(int categoryId) => _products.Where(p => p.CategoryId == categoryId && !p.IsDeleted).ToList();

        public List<Product> SearchProducts(string name = null, decimal? minPrice = null, decimal? maxPrice = null, int? categoryId = null)
        {
            var query = _products.Where(p => !p.IsDeleted).AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.IndexOf(name.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
            if (minPrice.HasValue)
                query = query.Where(p => p.SalePrice >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.SalePrice <= maxPrice.Value);
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);
            return query.ToList();
        }

        public void SoftDeleteProductsByCategory(int categoryId)
        {
            foreach (var p in _products.Where(p => p.CategoryId == categoryId))
                p.IsDeleted = true;

            _jsonHelper.Save(_products);
        }
    }
}
