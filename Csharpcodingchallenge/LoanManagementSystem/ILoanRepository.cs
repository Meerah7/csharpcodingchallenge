using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoanManagementSystem.Entity;

namespace LoanManagementSystem.DAO
{
    public interface ILoanRepository
    {
        void ApplyLoan(Loan loan);
        double CalculateInterest(int loanId);
        void UpdateLoanStatus(int loanId, string newStatus);

        double CalculateInterest(double principal, double rate, int term);
        string LoanStatus(int loanId);
        double CalculateEMI(int loanId);
        double CalculateEMI(double principal, double rate, int term);
        void LoanRepayment(int loanId, double amount);
        List<Loan> GetAllLoan();
        Loan GetLoanById(int loanId);
    }
}
