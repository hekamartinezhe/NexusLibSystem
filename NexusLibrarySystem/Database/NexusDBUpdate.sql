--DROP DATABASE IF EXISTS NexusDB;
--CREATE DATABASE NexusDB;

USE NexusDB;

-- Tabla de carreras
CREATE TABLE Major (
    majorId INT IDENTITY(1,1) PRIMARY KEY,
    majorName VARCHAR(50) NOT NULL
);

-- Tabla de usuarios
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

-- Tabla de categorías
CREATE TABLE Categories (
    categoryId INT IDENTITY(1,1) PRIMARY KEY, 
    categoryName VARCHAR(50) NOT NULL
);

-- Tabla de libros (actualizada)
CREATE TABLE Books (
    bookId INT IDENTITY(1,1) PRIMARY KEY,
    isbn VARCHAR(20) UNIQUE NOT NULL,
    categoryId INT,
    title VARCHAR(150) NOT NULL,
    author VARCHAR(100) NOT NULL,
    publisher VARCHAR(50),
    publicationYear VARCHAR(4),
    inventory INT DEFAULT 1,
    CONSTRAINT FK_Books_Categories FOREIGN KEY (categoryId) REFERENCES Categories(categoryId)
);

-- Tabla de préstamos
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

-- Datos iniciales
INSERT INTO Major (majorName) VALUES ('Computer Science'), ('Physics');

INSERT INTO Categories (categoryName) VALUES ('Science Fiction'), ('History'), ('Math');

INSERT INTO Categories (categoryName) VALUES ('Fantasy');
INSERT INTO Categories (categoryName) VALUES ('Mystery');
INSERT INTO Categories (categoryName) VALUES ('Biography');
INSERT INTO Categories (categoryName) VALUES ('Technology');
INSERT INTO Categories (categoryName) VALUES ('Philosophy');
INSERT INTO Categories (categoryName) VALUES ('Self-Help');
INSERT INTO Categories (categoryName) VALUES ('Children');
INSERT INTO Categories (categoryName) VALUES ('Art');

INSERT INTO Users (enrollementNum, fullName, quadrimester, pswdHash, userRole, majorId)
VALUES 
('admin', 'Admin', 6, '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Admin', 1),
('S1', 'Julissa Gutiérrez', 2, '6c213f7d5d7f47f0462a3df9d05a3d99f34d9c03d1d02dcd6e73f3fdb2c69385', 'Student', 2);

INSERT INTO Books (isbn, title, author, publisher, publicationYear, categoryId, inventory)
VALUES 
('9780140449136', 'The Odyssey', 'Homer', 'Penguin', '1999', 1, 5),
('9780140449266', 'The Iliad', 'Homer', 'Penguin', '2003', 1, 3),
('9780525566151', 'Sapiens', 'Yuval Noah Harari', 'Harper', '2015', 2, 4);
