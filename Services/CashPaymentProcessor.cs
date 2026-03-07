using ManagmentSystem.Contracts;
using ManagmentSystem.Exceptions;
using ManagmentSystem.Models;

namespace ManagmentSystem.Services
{
    public class CashPaymentProcessor : IPaymentProcessor
    {
        public Payment ProcessPayment(Order order, decimal receivedAmount)
        {
            if (receivedAmount < order.Total)
                throw new PaymentFailedException($"Insufficient funds. Required: {order.Total:N0}, Received: {receivedAmount:N0}");

            return new CashPayment
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Amount = order.Total,
                PaidAt = DateTime.Now,
                ReceivedAmount = receivedAmount
            };
        }
    }
}