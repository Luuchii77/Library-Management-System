USE [LibraryManagementSystem];
UPDATE [dbo].[Admins]
SET [Password] = 'Admin123' -- Replace with the actual hashed password string
WHERE [Username] = 'Admin';