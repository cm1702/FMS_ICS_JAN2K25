create database fms_db_ics
use fms_db_ics

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(30) NOT NULL,
    PhoneNumber NVARCHAR(15) UNIQUE NOT NULL,
    Email NVARCHAR(80) UNIQUE NOT NULL,
    Username NVARCHAR(20) UNIQUE NOT NULL,
    Password NVARCHAR(20) NOT NULL,
    Address NVARCHAR(150),
    DateOfBirth DATE,
    RegistrationDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending'
);

CREATE TABLE CardTypes (
    CardTypeID INT PRIMARY KEY IDENTITY(1,1),
    CardType NVARCHAR(20) UNIQUE NOT NULL,
    LimitAmount DECIMAL(18,2) NOT NULL,
    JoiningFee DECIMAL(18,2) NOT NULL
);


CREATE TABLE UserCards (
    CardID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    CardTypeID INT NOT NULL FOREIGN KEY REFERENCES CardTypes(CardTypeID),
    CardNumber NVARCHAR(16) UNIQUE NOT NULL,
    RemainingLimit DECIMAL(18,2),
    Validity DATE NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Inactive'
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    ProductDetails NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    ProcessingFee DECIMAL(18,2)
);

CREATE TABLE Purchases (
    PurchaseID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
    PurchaseDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    TenureMonths INT CHECK (TenureMonths IN (3, 6, 9, 12)),
    MonthlyEMI DECIMAL(18,2),
    RemainingEMI DECIMAL(18,2),
    LastPaidDate DATETIME
);

CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseID INT NOT NULL FOREIGN KEY REFERENCES Purchases(PurchaseID),
    TransactionDate DATETIME DEFAULT GETDATE(),
    AmountPaid DECIMAL(18,2) NOT NULL,
    RemainingAmount DECIMAL(18,2)
);

CREATE TABLE DocumentVerification (
    VerificationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    DocumentType NVARCHAR(50) NOT NULL,
    DocumentStatus NVARCHAR(20) DEFAULT 'Pending',
    Remarks NVARCHAR(250)
);

CREATE TABLE ForgotPasswordRequests (
    RequestID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    OTP NVARCHAR(10) NOT NULL,
    RequestTime DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending'
);

CREATE TABLE Admins (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(20) NOT NULL
);

CREATE TABLE AdminActions (
    ActionID INT PRIMARY KEY IDENTITY(1,1),
    AdminID INT NOT NULL FOREIGN KEY REFERENCES Admins(AdminID),
    ActionType NVARCHAR(30) NOT NULL,
    ActionDetails NVARCHAR(MAX),
    ActionDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE EligibilityCriteria (
    CriteriaID INT PRIMARY KEY IDENTITY(1,1),
    CardTypeID INT NOT NULL FOREIGN KEY REFERENCES CardTypes(CardTypeID),
    MinimumIncome DECIMAL(18,2),
    AgeLimit INT
);

INSERT INTO Users (FullName, PhoneNumber, Email, Username, Password, Address, DateOfBirth, Status)
VALUES 
('John Doe', '1234567890', 'john.doe@example.com', 'johndoe', 'password123', '123 Main St', '1990-01-01', 'Pending'),
('Jane Smith', '0987654321', 'jane.smith@example.com', 'janesmith', 'password456', '456 Elm St', '1985-05-15', 'Pending'),
('Alice Johnson', '1112223333', 'alice.johnson@example.com', 'alicej', 'password789', '789 Pine St', '1992-07-20', 'Pending'),
('Bob Brown', '4445556666', 'bob.brown@example.com', 'bobb', 'password012', '101 Oak St', '1980-03-10', 'Active'),
('Charlie White', '7778889999', 'charlie.white@example.com', 'charliew', 'password345', '202 Birch St', '1995-09-25', 'Pending'),
('Diana Green', '2223334444', 'diana.green@example.com', 'dianag', 'password678', '303 Maple St', '1988-11-05', 'Pending');

INSERT INTO CardTypes (CardType, LimitAmount, JoiningFee)
VALUES 
 ('Gold', 50000, 500),
 ('Titanium', 100000, 1000);

INSERT INTO UserCards (UserID, CardTypeID, CardNumber, RemainingLimit, Validity, Status)
VALUES
(1, 1, '1234567890123456', 50000, '2026-12-31', 'Active'),
(2, 2, '2345678901234567', 100000, '2027-05-15', 'Inactive'),
(3, 1, '3456789012345678', 40000, '2026-08-20', 'Active'),
(4, 2, '4567890123456789', 90000, '2026-03-10', 'Inactive'),
(5, 1, '5678901234567890', 50000, '2027-01-25', 'Active'),
(6, 2, '6789012345678901', 80000, '2027-07-05', 'Active');

INSERT INTO Products (ProductName, ProductDetails, Price, ProcessingFee)
VALUES
('Smartphone', 'Latest 5G smartphone', 50000, 500),
('Laptop', 'High-performance laptop', 80000, 800),
('Smartwatch', 'Fitness tracking smartwatch', 15000, 150),
('Headphones', 'Noise-canceling headphones', 20000, 200),
('Refrigerator', 'Double-door refrigerator', 60000, 600),
('Television', '55-inch 4K UHD Smart TV', 75000, 750);

INSERT INTO Purchases (UserID, ProductID, TotalAmount, TenureMonths, MonthlyEMI, RemainingEMI)
VALUES
(1, 1, 50000, 6, 8333.33, 50000),
(2, 2, 80000, 12, 6666.67, 80000),
(3, 3, 15000, 3, 5000.00, 15000),
(4, 4, 20000, 9, 2222.22, 20000),
(5, 5, 60000, 6, 10000.00, 60000),
(6, 6, 75000, 12, 6250.00, 75000);

INSERT INTO Transactions (PurchaseID, TransactionDate, AmountPaid, RemainingAmount)
VALUES
(1, '2025-01-01', 8333.33, 41666.67),
(1, '2025-02-01', 8333.33, 33333.34),
(2, '2025-01-01', 6666.67, 73333.33),
(3, '2025-01-01', 5000.00, 10000.00),
(4, '2025-01-01', 2222.22, 17777.78),
(5, '2025-01-01', 10000.00, 50000.00);

INSERT INTO DocumentVerification (UserID, DocumentType, DocumentStatus, Remarks)
VALUES
(1, 'ID Proof', 'Approved', 'Valid ID provided'),
(2, 'Bank Statement', 'Pending', 'Waiting for admin review'),
(3, 'Address Proof', 'Rejected', 'Insufficient details'),
(4, 'ID Proof', 'Approved', 'Verified successfully'),
(5, 'Bank Statement', 'Pending', 'Verification in progress'),
(6, 'Address Proof', 'Approved', 'All documents valid');

INSERT INTO ForgotPasswordRequests (UserID, OTP, Status)
VALUES
(1, '123456', 'Pending'),
(2, '654321', 'Verified'),
(3, '111111', 'Pending'),
(4, '222222', 'Pending'),
(5, '333333', 'Verified'),
(6, '444444', 'Pending');

INSERT INTO Admins (Username, Password)
VALUES
('admin1', 'adminpass1'),
('admin2', 'adminpass2');

INSERT INTO AdminActions (AdminID, ActionType, ActionDetails)
VALUES
(1, 'Activate User', 'Activated user ID 1'),
(1, 'Verify Document', 'Approved documents for user ID 4'),
(2, 'Deactivate User', 'Deactivated user ID 3'),
(2, 'Verify Document', 'Rejected documents for user ID 2');

INSERT INTO EligibilityCriteria (CardTypeID, MinimumIncome, AgeLimit)
VALUES
(1, 25000, 21),
(2, 50000, 25);

SELECT * FROM Users 
SELECT * FROM CardTypes
SELECT * FROM UserCards
SELECT * FROM Products
SELECT * FROM Purchases
SELECT * FROM Transactions
SELECT * FROM DocumentVerification
SELECT * FROM ForgotPasswordRequests
SELECT * FROM Admins
SELECT * FROM AdminActions
SELECT * FROM EligibilityCriteria


