using System;
using System.Linq;
using System.Collections.Generic;
using Spectre.Console;
using ManagmentSystem.Models;
using ManagmentSystem.Services;
using ManagmentSystem.Enums;
using ManagmentSystem.Views;
using AD_project.Contracts;
using AD_project.Views;

// ==========================
// Services Initialization
// ==========================
IProductService productService = new ProductService();
ICategoryService categoryService = new CategoryService(productService);
EmployeeService employeeService = new EmployeeService();
LoginService loginService = new LoginService();
OrderService orderService = new OrderService();
CartService cartService = new CartService(productService,orderService);
InvoiceService invoiceService = new InvoiceService();
ProductService productService1 = new ProductService();

// ==========================
// Views Initialization
// ==========================
var loginView = new LoginView();
var employeeMenuView = new EmployeeMenuView();
var productView = new ProductView();
var categoryView = new CategoryView();
var posView = new POSMenuView();
var orderView = new OrderMenuView();
var invoiceView = new InvoiceView();
var reportView = new ReportView();

// ==========================
// Main Program Loop
// ==========================
while (true)
{
    try
    {
        AnsiConsole.Clear();
        var (username, password) = loginView.ShowLoginScreen();
        var loggedEmployee = loginService.Authenticate(username, password);
        UserSession.CurrentEmployee = loggedEmployee;

        if (loggedEmployee.Role == UserRole.Manager)
            RunAdminModule();
        else
            RunEmployeeModule();
    }
    catch (Exception ex)
    {
        loginView.ShowError(ex.Message);
    }
}

// ==========================
// Admin Module
// ==========================
void RunAdminModule()
{
    bool logout = false;
    while (!logout)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[yellow]Admin Panel[/]").RuleStyle("blue"));

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a section:")
                .AddChoices("Manage Employees", "Manage Products", "Manage Categories", "View Orders/Invoices", "Reports", "Logout")
        );

        switch (choice)
        {
            case "Manage Employees": RunEmployeeManagement(); break;
            case "Manage Products": RunProductManagement(); break;
            case "Manage Categories": RunCategoryManagement(); break;
            case "View Orders/Invoices": RunOrdersModule(); break;
            case "Reports": RunReportsModule(); break;
            case "Logout": UserSession.CurrentEmployee = null; logout = true; break;
        }
    }
}

// ==========================
// Employee Module
// ==========================
void RunEmployeeModule()
{
    bool logout = false;
    while (!logout)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[blue]Employee Menu[/]").RuleStyle("white"));

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an action:")
                .AddChoices("POS - New Sale", "View Products", "View Orders/Invoices", "Logout")
        );

        switch (choice)
        {
            case "POS - New Sale": RunPOSModule(); break;
            case "View Products":
                productView.ShowProducts(productService.GetAllProducts(), categoryService.GetAllCategories());
                MessageView.Wait();
                break;
            case "View Orders/Invoices": RunOrdersModule(); break;
            case "Logout": UserSession.CurrentEmployee = null; logout = true; break;
        }
    }
}

// ==========================
// Employee Management
// ==========================
void RunEmployeeManagement()
{
    bool back = false;
    while (!back)
    {
        var choice = employeeMenuView.ShowMainMenu();
        try
        {
            switch (choice)
            {
                case "View All":
                    employeeMenuView.DisplayEmployeesTable(employeeService.GetAllEmployees());
                    MessageView.Wait();
                    break;
                case "Add":
                    var newData = employeeMenuView.GetNewEmployeeDetails();
                    var newEmp = new Employee(newData.user, newData.pass)
                    {
                        Username = newData.user,
                        Password = newData.pass,
                        PersonalInfo = new PersonalInfo { FirstName = newData.fname, LastName = newData.lname, PhoneNumber = newData.phone },
                        Role = newData.role
                    };
                    employeeService.AddEmployee(newEmp);
                    MessageView.ShowSuccess("Employee added successfully");
                    break;
                case "Delete":
                    employeeService.RemoveEmployee(employeeMenuView.GetUsernameToDelete());
                    MessageView.ShowSuccess("Employee deleted successfully");
                    break;
                case "Back": back = true; break;
            }
        }
        catch (Exception ex) { MessageView.ShowError(ex.Message); }
    }
}

// ==========================
// Category Management
// ==========================
void RunCategoryManagement()
{
    bool back = false;
    while (!back)
    {
        var choice = categoryView.ShowMenu();
        try
        {
            switch (choice)
            {
                case "View Categories":
                    categoryView.ShowCategories(categoryService.GetAllCategories());
                    MessageView.Wait();
                    break;
                case "Add Category":
                    categoryService.AddCategory(categoryView.AskCategoryName());
                    MessageView.ShowSuccess("Category added successfully");
                    break;
                case "Edit Category":
                    int? idEdit = categoryView.AskCategoryId();
                    if (idEdit.HasValue)
                    {
                        string newTitle = categoryView.AskCategoryName("Enter NEW category name:");
                        categoryService.UpdateCategory(idEdit.Value, newTitle);
                        MessageView.ShowSuccess("Category updated successfully");
                    }
                    break;
                case "Delete Category":
                    int? idDel = categoryView.AskCategoryId();
                    if (idDel.HasValue && categoryView.ConfirmDelete("Selected Category"))
                    {
                        categoryService.DeleteCategory(idDel.Value);
                        MessageView.ShowSuccess("Category deleted");
                    }
                    break;
                case "Back": back = true; break;
            }
        }
        catch (Exception ex) { MessageView.ShowError(ex.Message); }
    }
}

// ==========================
// Product Management Module
// ==========================
void RunProductManagement()
{
    bool back = false;
    while (!back)
    {
        var choice = productView.ShowProductsMenu();
        try
        {
            switch (choice)
            {
                case "Show All":
                    productView.ShowProducts(productService.GetAllProducts(), categoryService.GetAllCategories());
                    MessageView.Wait();
                    break;
                case "Add Product":
                    var details = productView.AskProductDetails(categoryService.GetAllCategories());
                    if (details.HasValue)
                    {
                        productService.AddProduct(details.Value.name, details.Value.desc, details.Value.qty, details.Value.salePrice, details.Value.costPrice, details.Value.catId);
                        MessageView.ShowSuccess("Product added successfully!");
                    }
                    break;
                case "Edit Product":
                    int idToEdit = productView.AskProductId("edit");
                    var existingProduct = productService.GetProductById(idToEdit);
                    if (existingProduct != null)
                    {
                        var updatedDetails = productView.AskProductDetails(categoryService.GetAllCategories());
                        if (updatedDetails.HasValue)
                        {
                            productService.UpdateProduct(idToEdit, updatedDetails.Value.name, updatedDetails.Value.desc, updatedDetails.Value.qty, updatedDetails.Value.salePrice, updatedDetails.Value.costPrice, updatedDetails.Value.catId);
                            MessageView.ShowSuccess("Product updated successfully!");
                        }
                    }
                    else MessageView.ShowError("Product not found!");
                    break;
                case "Delete Product":
                    int idToDelete = productView.AskProductId("delete");
                    if (AnsiConsole.Confirm($"Are you sure to delete product ID {idToDelete}?"))
                    {
                        bool deleted = productService.DeleteProduct(idToDelete);
                        MessageView.ShowSuccess(deleted ? "Product soft-deleted." : "Product not found.");
                    }
                    break;
                case "Search":
                    var searchTerm = AnsiConsole.Ask<string>("Enter product name to search:");
                    var results = productService.SearchProducts(searchTerm);
                    productView.ShowProducts(results, categoryService.GetAllCategories());
                    MessageView.Wait();
                    break;
                case "Back": back = true; break;
            }
        }
        catch (Exception ex) { MessageView.ShowError(ex.Message); }
    }
}

// ==========================
// POS / Cart Module
// ==========================
void RunPOSModule()
{
    bool back = false;
    while (!back)
    {
        var choice = posView.ShowMainMenu();
        var cart = cartService.GetCart();

        switch (choice)
        {
            case "Show Products":
                posView.ShowProducts(productService.GetAllProducts(), categoryService.GetAllCategories());
                posView.Wait();
                break;

            case "Add Product to Cart":
                int pid = posView.AskProductId("add");
                int qty = posView.AskQuantity("add");
                var product = productService.GetProductById(pid);
                if (product != null && !product.IsDeleted)
                {
                    cartService.AddToCart(product, qty);
                    posView.ShowMessage("Product added to cart");
                }
                else
                {
                    posView.ShowMessage("Product not found or deleted", false);
                }
                break;

            case "Remove Product from Cart":
                int rpid = posView.AskProductId("remove");
                cartService.RemoveItem(rpid);
                posView.ShowMessage("Product removed from cart");
                break;

            case "View Cart":
                posView.ShowCart(cart, categoryService.GetAllCategories());
                posView.Wait();
                break;

            case "Checkout / Payment":
                if (!cart.Items.Any())
                {
                    posView.ShowMessage("Cart is empty!", false);
                    break;
                }

                // طلب اسم الزبون
                string customerName = AnsiConsole.Ask<string>("Enter customer name:");

                // عرض المجموع
                decimal total = cart.Total;

                // استلام المبلغ المدفوع
                decimal receivedAmount = posView.AskPaymentAmount(total);

                if (receivedAmount < total)
                {
                    posView.ShowMessage("Received amount is less than total!", false);
                    break;
                }

                // إنشاء الطلب وحفظه
                var order = cartService.PlaceOrder(customerName); // السلة تُفرغ تلقائياً

                // حفظ الطلب في ملف الطلبات
                orderService.SavePaidOrder(order);

                // إنشاء الفاتورة وربطها بالموظف الحالي
                invoiceService.AddInvoice(new Invoice(order, UserSession.CurrentEmployee));

                posView.ShowMessage("Payment successful and invoice generated");
                break;

            case "Back":
                back = true;
                break;
        }
    }
}

// ==========================
// Orders / Invoices Module
// ==========================
void RunOrdersModule()
{
    bool back = false;
    while (!back)
    {
        var choice = orderView.ShowOrdersMenu();
        switch (choice)
        {
            case "Show All Paid Orders":
                orderView.ShowOrders(invoiceService.GetAllInvoices().Select(i => new Order
                {
                    Id = i.OrderId,
                    CustomerName = i.CustomerName,
                    Items = i.Items
                }).ToList());
                orderView.Wait();
                break;
            case "View Order Details":
                int? id = orderView.AskOrderId();
                if (id.HasValue)
                {
                    var invoice = invoiceService.GetAllInvoices().FirstOrDefault(i => i.OrderId == id.Value);
                    if (invoice != null)
                        orderView.ShowOrderDetails(invoiceToOrder(invoice), categoryService.GetAllCategories(), productService.GetAllProducts());
                    else orderView.ShowMessage("Order not found", false);
                }
                break;
            case "Back": back = true; break;
        }
    }

    Order invoiceToOrder(Invoice inv) => new Order
    {
        Id = inv.OrderId,
        CustomerName = inv.CustomerName,
        Items = inv.Items
    };
}

// ==========================
// Reports Module
// ==========================
void RunReportsModule()
{
    void RunReportsModule()
    {
        var reportView = new ReportView();
        bool back = false;

        while (!back)
        {
            // استدعاء المنيو من الـ View
            var choice = reportView.ShowReportsMenu();

            if (choice == "Monthly Sales & Profit Report")
            {
                // استدعاء دالة التقرير وتمرير الخدمات
                reportView.ShowMonthlyReport(invoiceService, productService1);
            }
            else if (choice == "Back")
            {
                back = true;
            }
        }
    }
}
