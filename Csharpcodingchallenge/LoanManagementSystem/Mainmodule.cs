
using System;
using System.Collections.Generic;
using LoanManagementSystem.DAO;
using LoanManagementSystem.Entity;
using LoanManagementSystem.Exception;



namespace LoanManagementSystem.Main
{
    class MainModule

    {
        static ILoanRepository loanRepo = new LoanRepositoryImpl();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n===== Loan Management System =====");
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Exit");
                Console.WriteLine("6.Loan Status");
                Console.Write("Enter choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ApplyLoan();
                        break;
                    case 2:
                        GetAllLoans();
                        break;
                    case 3:
                        GetLoanById();
                        break;
                    case 4:
                        LoanRepayment();
                        break;
                    case 5:
                        Console.WriteLine("Exiting... Thank you!");
                        return;
                    
                    case 6: CheckLoanStatus(); break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void ApplyLoan()
        {
            try
            {
                Console.Write("Enter Loan ID (e.g., 101): ");
                int loanId = int.Parse(Console.ReadLine());

                Console.Write("Enter Customer ID (e.g., 1): ");
                int customerId = int.Parse(Console.ReadLine());

                Console.Write("Enter Customer Name (e.g., John Doe): ");
                string name = Console.ReadLine();

                Console.Write("Enter Email (e.g., john.doe@example.com): ");
                string email = Console.ReadLine();

                Console.Write("Enter Phone (10-digit number, e.g., 9876543210): ");
                string phone = Console.ReadLine();

                Console.Write("Enter Address (e.g., Chennai, TN): ");
                string address = Console.ReadLine();

                Console.Write("Enter Credit Score (300 - 900): ");
                int creditScore = int.Parse(Console.ReadLine());

                Console.Write("Enter Principal Amount (e.g., 500000): ");
                double amount = double.Parse(Console.ReadLine());

                Console.Write("Enter Interest Rate (%) (e.g., 7.5): ");
                double rate = double.Parse(Console.ReadLine());

                Console.Write("Enter Loan Term (in months, e.g., 60): ");
                int term = int.Parse(Console.ReadLine());

                Console.Write("Enter Loan Type [CarLoan/HomeLoan]: ");
                string loanType = Console.ReadLine();

                Customer customer = new Customer(customerId, name, email, phone, address, creditScore);

                Loan loan;
                if (loanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter Car Model (e.g., Honda City): ");
                    string carModel = Console.ReadLine();

                    Console.Write("Enter Car Value (e.g., 800000): ");
                    int carValue = int.Parse(Console.ReadLine());

                    loan = new CarLoan(loanId, customer, amount, rate, term, loanType, "Pending", carModel, carValue);
                }
                else if (loanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter Property Address (e.g., Madurai, TN): ");
                    string propertyAddress = Console.ReadLine();

                    Console.Write("Enter Property Value (e.g., 4000000): ");
                    int propertyValue = int.Parse(Console.ReadLine());

                    loan = new HomeLoan(loanId, customer, amount, rate, term, loanType, "Pending", propertyAddress, propertyValue);
                }
                else
                {
                    Console.WriteLine("Invalid Loan Type. Must be 'CarLoan' or 'HomeLoan'. Application cancelled.");
                    return;
                }

                Console.Write("Confirm to apply loan (yes/no): ");
                string confirm = Console.ReadLine();

                if (confirm.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    loanRepo.ApplyLoan(loan);
                    Console.WriteLine("Loan applied successfully.");
                }
                else
                {
                    Console.WriteLine("Loan application cancelled.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter correct data types.");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error in applying loan: " + e.Message);
            }
        }


        static void GetAllLoans()
        {
            try
            {
                Console.WriteLine("\n📋 Fetching all loan records from the system...");
                List<Loan> loans = loanRepo.GetAllLoan();
                if (loans.Count == 0)
                {
                    Console.WriteLine("⚠️ No loans found in the system.");
                    return;
                }

                foreach (var loan in loans)
                {
                    loan.PrintDetails();
                    Console.WriteLine("--------------------------------------------");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("❌ Error fetching loans: " + ex.Message);
            }
        }


        static void GetLoanById()
        {
            try
            {
                Console.Write("\n🔍 Enter Loan ID to search (e.g., 101): ");
                if (int.TryParse(Console.ReadLine(), out int loanId))
                {
                    Loan loan = loanRepo.GetLoanById(loanId);
                    loan.PrintDetails();
                }
                else
                {
                    Console.WriteLine("❌ Invalid Loan ID format. Please enter a number.");
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine("⚠️ " + ex.Message);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("❌ Error while fetching loan: " + ex.Message);
            }
        }


        static void LoanRepayment()
        {
            try
            {
                Console.Write("\n💰 Enter Loan ID to repay (e.g., 102): ");
                if (!int.TryParse(Console.ReadLine(), out int loanId))
                {
                    Console.WriteLine("❌ Invalid Loan ID. Please enter a numeric value.");
                    return;
                }

                Console.Write("💵 Enter repayment amount (e.g., 10000): ");
                if (!double.TryParse(Console.ReadLine(), out double amount))
                {
                    Console.WriteLine("❌ Invalid amount format.");
                    return;
                }

                loanRepo.LoanRepayment(loanId, amount);
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine("⚠️ " + ex.Message);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("❌ Error in repayment: " + ex.Message);
            }
        }
        static void CheckLoanStatus()
        {
            try
            {
                Console.Write("\n🔎 Enter Loan ID to check status (e.g., 104): ");
                if (int.TryParse(Console.ReadLine(), out int loanId))
                {
                    string status = loanRepo.LoanStatus(loanId);
                    Console.WriteLine($"✅ Loan status for ID {loanId}: {status}");
                }
                else
                {
                    Console.WriteLine("❌ Invalid Loan ID format.");
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine("⚠️ " + ex.Message);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("❌ Error fetching loan status: " + ex.Message);
            }
        }


    }
}
