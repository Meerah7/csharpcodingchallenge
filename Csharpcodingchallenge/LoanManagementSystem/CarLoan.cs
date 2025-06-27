using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Entity
{
    public class CarLoan : Loan
    {
        public string CarModel { get; set; }
        public int CarValue { get; set; }

        public CarLoan() { }

        public CarLoan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm,
                        string loanType, string loanStatus, string carModel, int carValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanType, loanStatus)
        {
            CarModel = carModel;
            CarValue = carValue;
        }

        public override void PrintDetails()
        {
            Console.WriteLine($"{LoanId} | Car Loan | Model: {CarModel} | Value: {CarValue}");
        }
    }
}
