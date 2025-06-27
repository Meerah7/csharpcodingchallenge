using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LoanManagementSystem.DAO;
using LoanManagementSystem.Entity;
using LoanManagementSystem.Exception;

namespace LoanManagementSystem.Main
{
    class main
    {
        public static void Mainmenu(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Exit");
                Console.WriteLine("6. Check Loan Status");


                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        
                        Console.WriteLine("Apply Loan called");
                        break;
                    case "2":
                        
                        Console.WriteLine("Loan list called");
                        break;
                    case "3":
                        
                        Console.WriteLine("Get Loan Details called");
                        break;
                    case "4":
                        Console.WriteLine("Thank you! Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}
