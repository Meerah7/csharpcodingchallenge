CREATE DATABASE LoanDB;
GO

USE LoanDB;
GO

CREATE TABLE Customer (
    CustomerId INT PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(15),
    Address NVARCHAR(255),
    CreditScore INT
);

CREATE TABLE Loan (
    LoanId INT PRIMARY KEY,
    CustomerId INT,
    PrincipalAmount FLOAT,
    InterestRate FLOAT,
    LoanTerm INT,
    LoanType NVARCHAR(50),
    LoanStatus NVARCHAR(20),
    FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId)
);
INSERT INTO Customer VALUES (1, 'Arun Kumar', 'arun.kumar@example.com', '9876543210', 'Chennai, TN', 720);
INSERT INTO Customer VALUES (2, 'Divya Ramesh', 'divya.r@example.com', '9785612345', 'Madurai, TN', 680);
INSERT INTO Customer VALUES (3, 'Rahul Sharma', 'rahul.sharma@example.com', '9934578901', 'Coimbatore, TN', 640);
INSERT INTO Customer VALUES (4, 'Meena Iyer', 'meena.iyer@example.com', '9123456780', 'Salem, TN', 755);
INSERT INTO Customer VALUES (5, 'Vikram Raj', 'vikram.raj@example.com', '9988776655', 'Tirunelveli, TN', 590);
INSERT INTO Customer VALUES (6, 'Aishwarya D', 'aish.d@example.com', '9888877654', 'Trichy, TN', 710);
INSERT INTO Customer VALUES (7, 'Manoj Sekar', 'manoj.sekar@example.com', '9877890123', 'Erode, TN', 615);
INSERT INTO Customer VALUES (8, 'Lakshmi Narayan', 'lakshmi.n@example.com', '9911223344', 'Vellore, TN', 670);
INSERT INTO Customer VALUES (9, 'Priya Anbu', 'priya.anbu@example.com', '9845012345', 'Namakkal, TN', 705);
INSERT INTO Customer VALUES (10, 'Sathish K', 'sathish.k@example.com', '9789012345', 'Dindigul, TN', 800);


INSERT INTO Loan VALUES (101, 1, 500000, 7.5, 60, 'HomeLoan', 'Pending');
INSERT INTO Loan VALUES (102, 2, 250000, 8.0, 36, 'CarLoan', 'Pending');
INSERT INTO Loan VALUES (103, 3, 150000, 9.0, 24, 'CarLoan', 'Pending');
INSERT INTO Loan VALUES (104, 4, 1000000, 6.5, 120, 'HomeLoan', 'Pending');
INSERT INTO Loan VALUES (105, 5, 200000, 10.5, 48, 'CarLoan', 'Pending');
INSERT INTO Loan VALUES (106, 6, 800000, 7.0, 84, 'HomeLoan', 'Pending');
INSERT INTO Loan VALUES (107, 7, 300000, 9.5, 36, 'CarLoan', 'Done');
INSERT INTO Loan VALUES (108, 8, 1200000, 6.0, 180, 'HomeLoan', 'Pending');
INSERT INTO Loan VALUES (109, 9, 500000, 8.5, 60, 'CarLoan', 'Pending');
INSERT INTO Loan VALUES (110, 10, 2000000, 6.25, 240, 'HomeLoan', 'Done');
