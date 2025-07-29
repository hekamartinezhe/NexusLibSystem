--DROP DATABASE IF EXISTS NexusDB;
--CREATE DATABASE NexusDB;

USE NexusDB;


-- Create table Major
CREATE TABLE Major (
    majorId INT IDENTITY(1,1) PRIMARY KEY,
    majorName VARCHAR(50) NOT NULL
);

-- Create table Users
CREATE TABLE Users (
    userId INT IDENTITY(1,1) PRIMARY KEY,
    enrollementNum VARCHAR(10) UNIQUE,
    fullName VARCHAR(100) NOT NULL,
    quadrimester INT,
    pswdHash VARCHAR(255) NOT NULL,
    userRole VARCHAR(20) NOT NULL CHECK (userRole IN ('Admin', 'Student')),
    majorId INT,
    
    CONSTRAINT FK_Users_Major FOREIGN KEY (majorId) REFERENCES Major(majorId)
);

-- Create table Categories
CREATE TABLE Categories (
    categoryId INT IDENTITY(1,1) PRIMARY KEY, 
    categoryName VARCHAR(50) NOT NULL
);

-- Create table Books
CREATE TABLE Books (
    bookId INT IDENTITY(1,1) PRIMARY KEY,
    categoryId INT,
    title VARCHAR(150) NOT NULL,
    author VARCHAR(100) NOT NULL,
    publisher VARCHAR(50),
    publicationYear VARCHAR(4),
    
    CONSTRAINT FK_Books_Categories FOREIGN KEY (categoryId) REFERENCES Categories(categoryId)
);

-- Create table Loans
CREATE TABLE Loans (
    loanId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT NOT NULL,
    bookId INT NOT NULL,
    loanDate DATE NOT NULL,
    dueDate DATE NOT NULL,
    returnDate DATE,
    status VARCHAR(20) NOT NULL CHECK (status IN ('OnLoan', 'Returned', 'Overdue')),
    fineAmount DECIMAL(10, 2) DEFAULT 0,

    CONSTRAINT FK_Loans_Users FOREIGN KEY (userId) REFERENCES Users(userId),
    CONSTRAINT FK_Loans_Books FOREIGN KEY (bookId) REFERENCES Books(bookId)
);



