using ManagmentSystem.Models;

public sealed class Invoice
{
    public int Id { get; set; } 

    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }

    public int OrderId { get; set; }
    public DateTime InvoiceDate { get; set; }

    public string CustomerName { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }

    public Invoice() { }

    public Invoice(Order order, Employee employee)
    {
        OrderId = order.Id;
        CustomerName = order.CustomerName;
        Items = order.Items;
        Total = order.Total;
        InvoiceDate = DateTime.Now;

        EmployeeId = employee.Id;
        EmployeeName = employee.PersonalInfo.FullName;
    }
}