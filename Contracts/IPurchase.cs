using ManagmentSystem.Models;
using ManagmentSystem.Contracts;

namespace ManagmentSystem.Services
{
    public interface IPurchase
    {
        decimal CalculateTotal();
        Payment ProcessPayment(IPaymentProcessor processor, decimal receivedAmount);
        bool ConfirmOrder();
        List<OrderItem> GetItems();
    }
}
