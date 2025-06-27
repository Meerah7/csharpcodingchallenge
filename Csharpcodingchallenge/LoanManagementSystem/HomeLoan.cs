using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Entity
{
    public class HomeLoan : Loan
    {
        public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }

        public HomeLoan() { }

        public HomeLoan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm,
                        string loanType, string loanStatus, string propertyAddress, int propertyValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanType, loanStatus)
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }

        public override void PrintDetails()
        {
            Console.WriteLine($"{LoanId} | Home Loan | Property: {PropertyAddress} | Value: {PropertyValue}");
        }
    }
}
