using LoanManagementSystem.DAO;
using LoanManagementSystem.Entity;

class Program
{
    static void Main()
    {
        ILoanRepository loanRepo = new LoanRepositoryImpl();

        while (true)
        {
            Console.WriteLine("1. Apply Loan\n2. Get All Loans\n3. Get Loan by ID\n4. Loan Repayment\n5. Exit");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    // Call ApplyLoan()
                    break;
                case 2:
                    var loans = loanRepo.GetAllLoan();
                    loans.ForEach(l => l.PrintDetails());
                    break;
                case 3:
                    // Call GetLoanById
                    break;
                case 4:
                    // Call LoanRepayment
                    break;
                case 5:
                    return;
            }
        }
    }
}
