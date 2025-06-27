using NUnit.Framework;                         // ✅ NUnit for assertions and attributes
using LoanManagementSystem.DAO;                // ✅ Your project namespaces
using LoanManagementSystem.Entity;
using LoanManagementSystem.Exception;
using System.Collections.Generic;

namespace LoanManagementSystem.Tests           // ✅ Namespace should match the test project
{
    [TestFixture]                              // ✅ Marks this as a test class
    public class LoanRepositoryTests
    {
        // ✅ Declare test field ONLY ONCE
        private ILoanRepository loanRepo;

        [SetUp]                                 // ✅ Runs before every test
        public void Setup()
        {
            loanRepo = new LoanRepositoryImpl();  // Initialize repository before each test
        }

        [Test]
        public void ApplyLoan_ValidCarLoan_ShouldPass()
        {
            var customer = new Customer(101, "TestUser", "test@mail.com", "1234567890", "Chennai", 700);
            var loan = new CarLoan(1, customer, 100000, 8.5, 24, "CarLoan", "Pending", "Hyundai", 500000);

            Assert.DoesNotThrow(() => loanRepo.ApplyLoan(loan));  // ✅ Assertion using NUnit
        }

        [Test]
        public void CalculateInterest_ValidLoanId_ShouldReturnCorrectInterest()
        {
            var interest = loanRepo.CalculateInterest(1);
            Assert.Greater(interest, 0);                           // ✅ Valid assertion
        }

        [Test]
        public void GetLoanById_ValidId_ShouldReturnLoan()
        {
            var loan = loanRepo.GetLoanById(1);
            Assert.IsNotNull(loan);                                // ✅ Assert loan exists
            Assert.AreEqual(1, loan.LoanId);                       // ✅ Expected vs actual
        }

        // Add more tests similarly...
    }
}

