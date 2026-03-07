using System.Collections.Generic;
using ManagmentSystem.Models;

namespace ManagmentSystem.Services
{
    public interface IOrderService
    {
        void SavePaidOrder(Order order);
        List<Order> GetAllPaidOrders();
        Order GetOrderById(int id);
    }
}
