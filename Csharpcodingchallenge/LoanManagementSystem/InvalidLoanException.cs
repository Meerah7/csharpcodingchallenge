using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Exception
{
    public class InvalidLoanException : System.Exception
    {
        public InvalidLoanException(string message) : base(message) { }
    }
}

