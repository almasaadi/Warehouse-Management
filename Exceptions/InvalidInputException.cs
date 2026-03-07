using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Exceptions
{
    
        // General exception for any invalid input
        public class InvalidInputException : Exception
        {
            public InvalidInputException(string message) : base(message) { }
        }

       
    }


