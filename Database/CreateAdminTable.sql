USE [LibraryManagementSystem];

-- Create Admin table
CREATE TABLE [dbo].[Admins] (
    [AdminId] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(256) NOT NULL, -- For storing hashed password
    [ReferenceID] NVARCHAR(50) NOT NULL UNIQUE,
    [Email] NVARCHAR(100) NULL,
    [LastLoginDate] DATETIME NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE()
);

-- Insert default admin account
-- Password: Admin123 (will be hashed in the application)
INSERT INTO [dbo].[Admins] (Username, Password, ReferenceID, Email)
VALUES ('Admin', 'Admin123', 'ADMIN001', 'admin@library.com'); 