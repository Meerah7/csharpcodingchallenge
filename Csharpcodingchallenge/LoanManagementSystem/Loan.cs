using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Entity
{
    public abstract class Loan
    {
        public int LoanId { get; set; }
        public Customer Customer { get; set; }
        public double PrincipalAmount { get; set; }
        public double InterestRate { get; set; }
        public int LoanTerm { get; set; } // in months
        public string LoanType { get; set; }
        public string LoanStatus { get; set; } // Pending, Approved

        public Loan() { }

        public Loan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm, string loanType, string loanStatus)
        {
            LoanId = loanId;
            Customer = customer;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }

        public abstract void PrintDetails();
    }
}
