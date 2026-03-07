using AD_project.Contracts;
using ManagmentSystem.Data;
using ManagmentSystem.Models;

public class CategoryService : ICategoryService
{
    private readonly JsonHelper<Category> _helper;
    private readonly List<Category> _categories;
    private readonly IProductService _productService;

    public CategoryService(IProductService productService)
    {
        _helper = new JsonHelper<Category>("categories.json");
        _categories = _helper.Load();
        _productService = productService;
    }

    public List<Category> GetAllCategories()
        => _categories.Where(c => !c.IsDeleted).ToList();

    public void AddCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name required.");

        if (_categories.Any(c => c.Name.Equals(name,
            StringComparison.OrdinalIgnoreCase) && !c.IsDeleted))
            throw new InvalidOperationException("Category exists.");

        _categories.Add(new Category
        {
            Id = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1,
            Name = name.Trim()
        });

        _helper.Save(_categories);
    }

    public void UpdateCategory(int id, string newName)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        if (category == null)
            throw new InvalidOperationException("Category not found.");

        category.Name = newName.Trim();
        _helper.Save(_categories);
    }

    public void DeleteCategory(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        if (category == null)
            throw new InvalidOperationException("Category not found.");

        category.IsDeleted = true;
        _productService.SoftDeleteProductsByCategory(id);
        _helper.Save(_categories);
    }
}
