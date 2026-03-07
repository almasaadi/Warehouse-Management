using ManagmentSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagmentSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string CustomerName { get; set; } = "Guest";
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Status خاص، يمكن تغييره فقط من خلال وظائف الدفع أو الإلغاء
        public OrderStatus Status { get; private set; } = OrderStatus.PendingPayment;

        // المجموع الكلي للطلب محسوب ديناميكيًا
        public decimal Total => Items.Sum(i => i.SubTotal);

        // حالة الدفع
        public bool IsPaid => Status == OrderStatus.Paid;

        // تعيين الطلب كمُدفع
        public void MarkAsPaid()
        {
            if (IsPaid)
                throw new InvalidOperationException("Order is already paid.");

            Status = OrderStatus.Paid;
            OrderDate = DateTime.Now; // يمكن تحديث وقت الدفع
        }

        // إلغاء الطلب
        public void Cancel()
        {
            if (IsPaid)
                throw new InvalidOperationException("Cannot cancel a paid order.");

            Status = OrderStatus.Cancelled;
        }
    }
}
