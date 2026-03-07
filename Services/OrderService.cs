using System;
using System.Collections.Generic;
using System.Linq;
using ManagmentSystem.Models;
using ManagmentSystem.Data;

namespace ManagmentSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly JsonHelper<Order> _jsonHelper;
        private readonly List<Order> _orders;

        public OrderService()
        {
            _jsonHelper = new JsonHelper<Order>("orders.json"); // ملف خاص بالطلبات
            _orders = _jsonHelper.Load();
        }

        // حفظ الطلب المدفوع
        public void SavePaidOrder(Order order)
        {
            if (!order.IsPaid)
                throw new InvalidOperationException("Cannot save unpaid order.");

            if (string.IsNullOrWhiteSpace(order.CustomerName))
                throw new InvalidOperationException("Customer name is required.");

            // إعطاء رقم فريد للطلب
            order.Id = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;

            _orders.Add(order);
            _jsonHelper.Save(_orders);
        }

        // استرجاع كل الطلبات المدفوعة
        public List<Order> GetAllPaidOrders()
        {
            return _orders.Where(o => o.IsPaid).ToList();
        }

        // استرجاع طلب حسب المعرف
        public Order GetOrderById(int id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }
    }
}
