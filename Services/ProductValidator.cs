using System;
using ManagmentSystem.Models;
using Spectre.Console;
using ManagmentSystem.Extensions;
using AD_project.Contracts;

namespace AD_project.Services
{ 
    public class ProductValidator : IProductValidator
    {
        public void ValidateForCreateOrUpdate(Product product)
        {
            if (!product.Name.IsValidProductName())
                throw new ValidationException("Product name is required and must be ≤100 characters.");

            if (!string.IsNullOrWhiteSpace(product.Description) && !product.Description.IsValidDescription())
                throw new ValidationException("Description must be ≤500 characters if provided.");

            if (product.Quantity < 0)
                throw new ValidationException("Quantity cannot be negative.");

            if (product.SalePrice < 0)
                throw new ValidationException("Sale Price cannot be negative.");

            if (product.CostPrice < 0)
                throw new ValidationException("Cost Price cannot be negative.");
        }
    }


    // استثناء مخصص للتحقق
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}