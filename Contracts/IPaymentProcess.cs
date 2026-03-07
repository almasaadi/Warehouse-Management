using ManagmentSystem.Enums;
using ManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Contracts
{
    public interface IPaymentProcessor
    {
   
        Payment ProcessPayment(Order order, decimal receivedAmount);
    }
}
