using LoanManagementSystem.Entity;
using LoanManagementSystem.Exception;
using LoanManagementSystem.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LoanManagementSystem.DAO
{

    public class GenericLoan : Loan
    {
        public GenericLoan(int loanId, Customer customer, double principalAmount, double interestRate, int loanTerm, string loanType, string loanStatus)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanType, loanStatus)
        {
        }

        public override void PrintDetails()
        {
            // Implement how you want to print details of GenericLoan
            Console.WriteLine($"Loan ID: {LoanId}, Principal: {PrincipalAmount}, Status: {LoanStatus}");
        }
    }
    public class LoanRepositoryImpl : ILoanRepository
    {
        public void ApplyLoan(Loan loan)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();

                // Store customer first if not already stored
                SqlCommand custCmd = new SqlCommand("IF NOT EXISTS (SELECT * FROM Customer WHERE CustomerId = @CustomerId) " +
                                                    "INSERT INTO Customer VALUES (@CustomerId, @Name, @Email, @Phone, @Address, @CreditScore)", conn);
                custCmd.Parameters.AddWithValue("@CustomerId", loan.Customer.CustomerId);
                custCmd.Parameters.AddWithValue("@Name", loan.Customer.Name);
                custCmd.Parameters.AddWithValue("@Email", loan.Customer.Email);
                custCmd.Parameters.AddWithValue("@Phone", loan.Customer.Phone);
                custCmd.Parameters.AddWithValue("@Address", loan.Customer.Address);
                custCmd.Parameters.AddWithValue("@CreditScore", loan.Customer.CreditScore);
                custCmd.ExecuteNonQuery();

                // Insert loan
                SqlCommand cmd = new SqlCommand("INSERT INTO Loan VALUES (@LoanId, @CustomerId, @Amount, @Rate, @Term, @Type, @Status)", conn);
                cmd.Parameters.AddWithValue("@LoanId", loan.LoanId);
                cmd.Parameters.AddWithValue("@CustomerId", loan.Customer.CustomerId);
                cmd.Parameters.AddWithValue("@Amount", loan.PrincipalAmount);
                cmd.Parameters.AddWithValue("@Rate", loan.InterestRate);
                cmd.Parameters.AddWithValue("@Term", loan.LoanTerm);
                cmd.Parameters.AddWithValue("@Type", loan.LoanType);
                cmd.Parameters.AddWithValue("@Status", "Pending");
                cmd.ExecuteNonQuery();
            }
        }

        public double CalculateInterest(int loanId)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId", conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        double principal = Convert.ToDouble(reader["PrincipalAmount"]);
                        double rate = Convert.ToDouble(reader["InterestRate"]);
                        int term = Convert.ToInt32(reader["LoanTerm"]);
                        return CalculateInterest(principal, rate, term);
                    }
                    else
                    {
                        throw new InvalidLoanException($"Loan ID {loanId} not found.");
                    }
                }
            }
        }

        public double CalculateInterest(double principal, double rate, int term)
        {
            return (principal * rate * term) / 12;
        }

        public double CalculateEMI(int loanId)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId", conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        double principal = Convert.ToDouble(reader["PrincipalAmount"]);
                        double rate = Convert.ToDouble(reader["InterestRate"]);
                        int term = Convert.ToInt32(reader["LoanTerm"]);
                        return CalculateEMI(principal, rate, term);
                    }
                    else
                    {
                        throw new InvalidLoanException($"Loan ID {loanId} not found.");
                    }
                }
            }
        }

        public double CalculateEMI(double principal, double rate, int term)
        {
            double monthlyRate = rate / 12 / 100;
            return (principal * monthlyRate * Math.Pow(1 + monthlyRate, term)) / (Math.Pow(1 + monthlyRate, term) - 1);
        }

        public string LoanStatus(int loanId)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT c.CreditScore FROM Loan l JOIN Customer c ON l.CustomerId = c.CustomerId WHERE l.LoanId = @LoanId", conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);

                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    throw new InvalidLoanException($"Loan ID {loanId} not found.");
                }

                int creditScore = Convert.ToInt32(result);
                string status = creditScore > 650 ? "Approved" : "Rejected";

                SqlCommand update = new SqlCommand("UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId", conn);
                update.Parameters.AddWithValue("@Status", status);
                update.Parameters.AddWithValue("@LoanId", loanId);
                update.ExecuteNonQuery();

                return status;
            }
        }

        public void LoanRepayment(int loanId, double amount)
        {
            double emi = CalculateEMI(loanId);
            if (amount < emi)
            {
                Console.WriteLine("Amount is less than one EMI. Payment rejected.");
                return;
            }

            int emiCount = (int)(amount / emi);
            Console.WriteLine($"Payment accepted. {emiCount} EMI(s) will be paid from this amount.");
            // Update loan balance or EMI count logic here if needed
        }

        public List<Loan> GetAllLoan()
        {
            List<Loan> loanList = new List<Loan>();

            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT l.*, c.Name, c.Email, c.Phone, c.Address, c.CreditScore FROM Loan l JOIN Customer c ON l.CustomerId = c.CustomerId", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer cust = new Customer(
                            Convert.ToInt32(reader["CustomerId"]),
                            reader["Name"].ToString(),
                            reader["Email"].ToString(),
                            reader["Phone"].ToString(),
                            reader["Address"].ToString(),
                            Convert.ToInt32(reader["CreditScore"])
                        );

                        string type = reader["LoanType"].ToString();

                        Loan loan;
                        if (type == "CarLoan")
                        {
                            loan = new CarLoan(
                                Convert.ToInt32(reader["LoanId"]),
                                cust,
                                Convert.ToDouble(reader["PrincipalAmount"]),
                                Convert.ToDouble(reader["InterestRate"]),
                                Convert.ToInt32(reader["LoanTerm"]),
                                type,
                                reader["LoanStatus"].ToString(),
                                "ModelX", 500000); // Ideally, fetch actual car details if stored
                        }
                        else if (type == "HomeLoan")
                        {
                            loan = new HomeLoan(
                                Convert.ToInt32(reader["LoanId"]),
                                cust,
                                Convert.ToDouble(reader["PrincipalAmount"]),
                                Convert.ToDouble(reader["InterestRate"]),
                                Convert.ToInt32(reader["LoanTerm"]),
                                type,
                                reader["LoanStatus"].ToString(),
                                "ABC Street", 2000000); // Ideally, fetch actual property details if stored
                        }
                        else
                        {
                            loan = new GenericLoan(
                                Convert.ToInt32(reader["LoanId"]),
                                cust,
                                Convert.ToDouble(reader["PrincipalAmount"]),
                                Convert.ToDouble(reader["InterestRate"]),
                                Convert.ToInt32(reader["LoanTerm"]),
                                type,
                                reader["LoanStatus"].ToString()
                            );
                        }

                        loanList.Add(loan);
                    }
                }
            }

            return loanList;
        }

        public Loan GetLoanById(int loanId)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT l.*, c.Name, c.Email, c.Phone, c.Address, c.CreditScore FROM Loan l JOIN Customer c ON l.CustomerId = c.CustomerId WHERE l.LoanId = @LoanId", conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Customer cust = new Customer(
                            Convert.ToInt32(reader["CustomerId"]),
                            reader["Name"].ToString(),
                            reader["Email"].ToString(),
                            reader["Phone"].ToString(),
                            reader["Address"].ToString(),
                            Convert.ToInt32(reader["CreditScore"])
                        );

                        string type = reader["LoanType"].ToString();

                        if (type == "CarLoan")
                        {
                            return new CarLoan(
                                Convert.ToInt32(reader["LoanId"]),
                                cust,
                                Convert.ToDouble(reader["PrincipalAmount"]),
                                Convert.ToDouble(reader["InterestRate"]),
                                Convert.ToInt32(reader["LoanTerm"]),
                                type,
                                reader["LoanStatus"].ToString(),
                                "ModelX", 500000); // Ideally, fetch actual car details
                        }
                        else if (type == "HomeLoan")
                        {
                            return new HomeLoan(
                                Convert.ToInt32(reader["LoanId"]),
                                cust,
                                Convert.ToDouble(reader["PrincipalAmount"]),
                                Convert.ToDouble(reader["InterestRate"]),
                                Convert.ToInt32(reader["LoanTerm"]),
                                type,
                                reader["LoanStatus"].ToString(),
                                "ABC Street", 2000000); // Ideally, fetch actual property details
                        }
                        else
                        {
                            return new GenericLoan(
                                Convert.ToInt32(reader["LoanId"]),
                                cust,
                                Convert.ToDouble(reader["PrincipalAmount"]),
                                Convert.ToDouble(reader["InterestRate"]),
                                Convert.ToInt32(reader["LoanTerm"]),
                                type,
                                reader["LoanStatus"].ToString()
                            );
                        }
                    }
                    else
                    {
                        throw new InvalidLoanException($"Loan ID {loanId} not found.");
                    }
                }
            }
        }

        public void UpdateLoanStatus(int loanId, string newStatus)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                string sql = "UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                        throw new InvalidLoanException("Loan not found with ID: " + loanId);
                }
            }
        }
    }
}
