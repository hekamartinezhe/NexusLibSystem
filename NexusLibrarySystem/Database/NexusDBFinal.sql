--DROP DATABASE IF EXISTS NexusDB;
--CREATE DATABASE NexusDB;

USE NexusDB;

-- ========================
-- TABLAS
-- ========================

-- Tabla de carreras
CREATE TABLE Major (
    majorId INT IDENTITY(1,1) PRIMARY KEY,
    majorName VARCHAR(50) NOT NULL
);

-- Tabla de usuarios
CREATE TABLE Users (
    userId INT IDENTITY(1,1) PRIMARY KEY,
    enrollmentNum VARCHAR(10) UNIQUE,
    fullName VARCHAR(100) NOT NULL,
    quadrimester INT,
    pswdHash VARCHAR(255) NOT NULL,
    userRole VARCHAR(20) NOT NULL CHECK (userRole IN ('Admin', 'Student')),
    majorId INT,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Users_Major FOREIGN KEY (majorId) REFERENCES Major(majorId)
);

-- Tabla de categorías
CREATE TABLE Categories (
    categoryId INT IDENTITY(1,1) PRIMARY KEY, 
    categoryName VARCHAR(50) NOT NULL
);

-- Tabla de libros
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

-- ========================
-- DATOS INICIALES
-- ========================

-- Carreras
INSERT INTO Major (majorName) 
VALUES ('Computer Science'), ('Physics');

-- Categorías
INSERT INTO Categories (categoryName) VALUES 
('Science Fiction'), ('History'), ('Math'),
('Fantasy'), ('Mystery'), ('Biography'),
('Technology'), ('Philosophy'), ('Self-Help'),
('Children'), ('Art'), 
('Novel'), ('Classic Literature');

-- Usuarios (Julissa, Jocelyn, Javier, Kaleb y Admin)
INSERT INTO Users (enrollmentNum, fullName, quadrimester, pswdHash, userRole, majorId)
VALUES 
('S1', 'Julissa Gutiérrez', 2, '264c8c381bf16c982a4e59b0dd4c6f7808c51a05f64c35db42cc78a2a72875bb', 'Student', 2),
('S2', 'Jocelyn Bañuelos', 3, '264c8c381bf16c982a4e59b0dd4c6f7808c51a05f64c35db42cc78a2a72875bb', 'Student', 2),
('S3', 'Javier Ayón', 3, '264c8c381bf16c982a4e59b0dd4c6f7808c51a05f64c35db42cc78a2a72875bb', 'Student', 2),
('S4', 'Kaleb Martínez', 6, '264c8c381bf16c982a4e59b0dd4c6f7808c51a05f64c35db42cc78a2a72875bb', 'Student', 1),
('admin', 'Admin User', 6, '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Admin', 1);

-- Libros
INSERT INTO Books (isbn, title, author, publisher, publicationYear, categoryId, inventory)
VALUES 
('9780140449136', 'The Odyssey', 'Homer', 'Penguin', '1999', 1, 5),
('9780140449266', 'The Iliad', 'Homer', 'Penguin', '2003', 1, 3),
('9780525566151', 'Sapiens', 'Yuval Noah Harari', 'Harper', '2015', 2, 4),
('9780060531041', 'Cien años de soledad', 'Gabriel García Márquez', 'Harper', '1967', 6, 6);

-- ========================
-- PRÉSTAMOS (Loans)
-- ========================

-- Activo (OnLoan) - Julissa
INSERT INTO Loans (userId, bookId, loanDate, dueDate, returnDate, status, fineAmount)
VALUES (1, 1, GETDATE(), DATEADD(DAY, 7, GETDATE()), NULL, 'OnLoan', 0);

-- Devuelto (Returned) - Jocelyn
INSERT INTO Loans (userId, bookId, loanDate, dueDate, returnDate, status, fineAmount)
VALUES (2, 2, DATEADD(DAY, -15, GETDATE()), DATEADD(DAY, -8, GETDATE()), DATEADD(DAY, -5, GETDATE()), 'Returned', 0);

-- Vencido (Overdue) - Javier
INSERT INTO Loans (userId, bookId, loanDate, dueDate, returnDate, status, fineAmount)
VALUES (3, 3, DATEADD(DAY, -20, GETDATE()), DATEADD(DAY, -13, GETDATE()), NULL, 'Overdue', 70);

-- Otro préstamo activo - Kaleb
INSERT INTO Loans (userId, bookId, loanDate, dueDate, returnDate, status, fineAmount)
VALUES (4, 4, GETDATE(), DATEADD(DAY, 7, GETDATE()), NULL, 'OnLoan', 0);
