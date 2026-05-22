-- 1. Create the Categories Table
CREATE TABLE tblCategories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255)
);

-- 2. Create the Resources Table with a Foreign Key
CREATE TABLE tblResources (
    ResourceID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100),
    ResourceURL NVARCHAR(MAX) NOT NULL,
    DateAdded DATETIME DEFAULT GETDATE(),
    CategoryID INT,
    CONSTRAINT FK_Category FOREIGN KEY (CategoryID) 
    REFERENCES tblCategories(CategoryID)
);

-- 1. Create the Categories Table
CREATE TABLE tblCategories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255)
);

-- 2. Create the Resources Table with a Foreign Key
CREATE TABLE tblResources (
    ResourceID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100),
    ResourceURL NVARCHAR(MAX) NOT NULL,
    DateAdded DATETIME DEFAULT GETDATE(),
    CategoryID INT,
    CONSTRAINT FK_Category FOREIGN KEY (CategoryID) 
    REFERENCES tblCategories(CategoryID)
);

USE SGD4_ResourceTracker

DELETE FROM tblResources  -- clears all rows
DBCC CHECKIDENT ('tblResources', RESEED, 0)  -- resets ID back to 1