-- Drop existing tables if they exist (ensure order to prevent FK issues)
IF OBJECT_ID('Transactions', 'U') IS NOT NULL DROP TABLE Transactions;
IF OBJECT_ID('Students', 'U') IS NOT NULL DROP TABLE Students;
IF OBJECT_ID('Books', 'U') IS NOT NULL DROP TABLE Books;

-- Books Table
CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1
);

-- Students Table (Updated)
CREATE TABLE Students (
    StudentId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ReferenceID NVARCHAR(11) NOT NULL UNIQUE, -- Required & Unique
    Email NVARCHAR(100) NULL,                 -- Optional
    PhoneNumber NVARCHAR(20) NULL             -- Optional
);

-- Transactions Table (with DueDate)
CREATE TABLE Transactions (
    TransactionId INT PRIMARY KEY IDENTITY(1,1),
    BookId INT NOT NULL,
    StudentId INT NOT NULL,
    BorrowDate DATETIME2 NOT NULL,
    ReturnDate DATETIME2 NULL,
    DueDate DATETIME2 NULL,  -- <--- Use this for the due date
    FOREIGN KEY (BookId) REFERENCES Books(BookId) ON DELETE CASCADE,
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE
);

-- Indexes for optimization
CREATE INDEX idx_BookId ON Transactions(BookId);
CREATE INDEX idx_StudentId ON Transactions(StudentId);