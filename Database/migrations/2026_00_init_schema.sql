-- Migration 2026_00: Initial schema migration for RMS
-- This migration creates the full schema and seeds sample data. Generated from Database/init_rms.sql

IF DB_ID(N'RMS') IS NULL
BEGIN
    PRINT 'Creating database [RMS]...';
    CREATE DATABASE [RMS];
END
GO

USE [RMS];
GO

-- ----------------------
-- Users
-- ----------------------
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
CREATE TABLE dbo.Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NULL,
    DisplayName NVARCHAR(100) NULL,
    Role TINYINT NOT NULL DEFAULT(2), -- 0=Manager,1=Chef,2=Service
    IsActive BIT NOT NULL DEFAULT(1),
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ----------------------
-- Tables (Dining tables)
-- ----------------------
IF OBJECT_ID(N'dbo.Tables', N'U') IS NOT NULL DROP TABLE dbo.Tables;
CREATE TABLE dbo.Tables
(
    TableId INT IDENTITY(1,1) PRIMARY KEY,
    Code NVARCHAR(20) NOT NULL UNIQUE,
    Location NVARCHAR(40) NULL,
    Capacity TINYINT NOT NULL DEFAULT(0),
    Status TINYINT NOT NULL DEFAULT(0), -- 0=Available,1=Reserved,2=Occupied,3=NeedsCleaning,4=OutOfService
    Notes NVARCHAR(200) NULL
);
GO

-- ----------------------
-- Reservations
-- ----------------------
IF OBJECT_ID(N'dbo.Reservations', N'U') IS NOT NULL DROP TABLE dbo.Reservations;
CREATE TABLE dbo.Reservations
(
    ReservationId INT IDENTITY(1,1) PRIMARY KEY,
    TableId INT NOT NULL,
    CustomerName NVARCHAR(120) NULL,
    CustomerPhone NVARCHAR(30) NULL,
    PartySize TINYINT NULL,
    ReservedFromUtc DATETIME2 NOT NULL,
    ReservedToUtc DATETIME2 NULL,
    Status TINYINT NOT NULL DEFAULT(0), -- 0=Booked,1=Seated,2=Completed,3=Cancelled,4=NoShow
    Notes NVARCHAR(200) NULL,
    CONSTRAINT FK_Reservations_Table FOREIGN KEY (TableId) REFERENCES dbo.Tables(TableId)
);
GO

-- ----------------------
-- MenuCategories
-- ----------------------
IF OBJECT_ID(N'dbo.MenuCategories', N'U') IS NOT NULL DROP TABLE dbo.MenuCategories;
CREATE TABLE dbo.MenuCategories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(80) NOT NULL,
    SortOrder INT NOT NULL DEFAULT(0)
);
GO

-- ----------------------
-- MenuItems
-- ----------------------
IF OBJECT_ID(N'dbo.MenuItems', N'U') IS NOT NULL DROP TABLE dbo.MenuItems;
CREATE TABLE dbo.MenuItems
(
    MenuItemId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryId INT NULL,
    Name NVARCHAR(120) NOT NULL,
    Description NVARCHAR(400) NULL,
    Price DECIMAL(10,2) NOT NULL,
    Size NVARCHAR(50) NULL,
    ImageFile NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT(1),
    CONSTRAINT FK_MenuItems_Category FOREIGN KEY (CategoryId) REFERENCES dbo.MenuCategories(CategoryId)
);
GO

-- ----------------------
-- TableSessions (group multiple orders per seating)
-- ----------------------
IF OBJECT_ID(N'dbo.TableSessions', N'U') IS NOT NULL DROP TABLE dbo.TableSessions;
CREATE TABLE dbo.TableSessions
(
    SessionId INT IDENTITY(1,1) PRIMARY KEY,
    TableId INT NOT NULL,
    ReservationId INT NULL,
    StartedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    EndedAtUtc DATETIME2 NULL,
    Status TINYINT NOT NULL DEFAULT(0), -- 0=Open,1=Closed
    CreatedByUserId INT NULL,
    Notes NVARCHAR(400) NULL,
    CONSTRAINT FK_TableSessions_Table FOREIGN KEY (TableId) REFERENCES dbo.Tables(TableId),
    CONSTRAINT FK_TableSessions_Reservation FOREIGN KEY (ReservationId) REFERENCES dbo.Reservations(ReservationId),
    CONSTRAINT FK_TableSessions_CreatedBy FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(UserId)
);
GO
CREATE INDEX IX_TableSessions_TableId ON dbo.TableSessions(TableId);
GO

-- ----------------------
-- Orders
-- ----------------------
IF OBJECT_ID(N'dbo.Orders', N'U') IS NOT NULL DROP TABLE dbo.Orders;
CREATE TABLE dbo.Orders
(
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    OrderNumber NVARCHAR(20) NULL,
    TableId INT NOT NULL,
    SessionId INT NULL,
    ReservationId INT NULL,
    RoundNumber INT NOT NULL DEFAULT(1), -- grouping/order round within a session
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Status TINYINT NOT NULL DEFAULT(0), -- 0=Open,1=InKitchen,2=Ready,3=Closed,4=Cancelled
    CreatedByUserId INT NULL,
    ClosedByUserId INT NULL,
    GuestCount TINYINT NULL,
    Notes NVARCHAR(400) NULL,
    Subtotal DECIMAL(10,2) NOT NULL DEFAULT(0),
    Tax DECIMAL(10,2) NOT NULL DEFAULT(0),
    Total DECIMAL(10,2) NOT NULL DEFAULT(0),
    IsPaid BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_Orders_Table FOREIGN KEY (TableId) REFERENCES dbo.Tables(TableId),
    CONSTRAINT FK_Orders_Session FOREIGN KEY (SessionId) REFERENCES dbo.TableSessions(SessionId),
    CONSTRAINT FK_Orders_Reservation FOREIGN KEY (ReservationId) REFERENCES dbo.Reservations(ReservationId),
    CONSTRAINT FK_Orders_CreatedBy FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Orders_ClosedBy FOREIGN KEY (ClosedByUserId) REFERENCES dbo.Users(UserId)
);
GO
CREATE INDEX IX_Orders_SessionId ON dbo.Orders(SessionId);
GO

-- ----------------------
-- OrderLines
-- ----------------------
IF OBJECT_ID(N'dbo.OrderLines', N'U') IS NOT NULL DROP TABLE dbo.OrderLines;
CREATE TABLE dbo.OrderLines
(
    OrderLineId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    MenuItemId INT NOT NULL,
    Quantity DECIMAL(6,2) NOT NULL DEFAULT(1),
    UnitPrice DECIMAL(10,2) NOT NULL,
    LineNotes NVARCHAR(200) NULL,
    Status TINYINT NOT NULL DEFAULT(0), -- 0=Pending,1=Cooking,2=Ready,3=Served
    LineTotal AS (Quantity * UnitPrice) PERSISTED,
    CONSTRAINT FK_OrderLines_Order FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_OrderLines_MenuItem FOREIGN KEY (MenuItemId) REFERENCES dbo.MenuItems(MenuItemId)
);
GO

-- ----------------------
-- Payments
-- ----------------------
IF OBJECT_ID(N'dbo.Payments', N'U') IS NOT NULL DROP TABLE dbo.Payments;
CREATE TABLE dbo.Payments
(
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NULL,
    SessionId INT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    PaymentType TINYINT NOT NULL, -- 0=Cash,1=Card,2=QR/Wallet
    PaidAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ProcessedByUserId INT NULL,
    Reference NVARCHAR(50) NULL,
    IsRefund BIT NOT NULL DEFAULT(0),
    RelatedPaymentId INT NULL,
    CONSTRAINT FK_Payments_Order FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId),
    CONSTRAINT FK_Payments_Session FOREIGN KEY (SessionId) REFERENCES dbo.TableSessions(SessionId),
    CONSTRAINT FK_Payments_ProcessedBy FOREIGN KEY (ProcessedByUserId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Payments_RelatedPayment FOREIGN KEY (RelatedPaymentId) REFERENCES dbo.Payments(PaymentId)
);
GO
CREATE INDEX IX_Payments_SessionId ON dbo.Payments(SessionId);
GO

-- ----------------------
-- InventoryCategories
-- ----------------------
IF OBJECT_ID(N'dbo.InventoryCategories', N'U') IS NOT NULL DROP TABLE dbo.InventoryCategories;
CREATE TABLE dbo.InventoryCategories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL
);
GO

-- ----------------------
-- InventorySubcategories
-- ----------------------
IF OBJECT_ID(N'dbo.InventorySubcategories', N'U') IS NOT NULL DROP TABLE dbo.InventorySubcategories;
CREATE TABLE dbo.InventorySubcategories
(
    SubcategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryId INT NOT NULL,
    Name NVARCHAR(120) NOT NULL,
    CONSTRAINT FK_InventorySubcategories_Category FOREIGN KEY (CategoryId) REFERENCES dbo.InventoryCategories(CategoryId)
);
GO

-- ----------------------
-- InventoryItems
-- ----------------------
IF OBJECT_ID(N'dbo.InventoryItems', N'U') IS NOT NULL DROP TABLE dbo.InventoryItems;
CREATE TABLE dbo.InventoryItems
(
    InventoryItemId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryId INT NULL,
    SubcategoryId INT NULL,
    Name NVARCHAR(120) NOT NULL,
    Unit NVARCHAR(20) NULL,
    CurrentStock DECIMAL(12,3) NOT NULL DEFAULT(0),
    ReorderLevel DECIMAL(12,3) NOT NULL DEFAULT(0),
    CONSTRAINT FK_InventoryItems_Category FOREIGN KEY (CategoryId) REFERENCES dbo.InventoryCategories(CategoryId),
    CONSTRAINT FK_InventoryItems_Subcategory FOREIGN KEY (SubcategoryId) REFERENCES dbo.InventorySubcategories(SubcategoryId)
);
GO

-- ----------------------
-- InventoryTransactions
-- ----------------------
IF OBJECT_ID(N'dbo.InventoryTransactions', N'U') IS NOT NULL DROP TABLE dbo.InventoryTransactions;
CREATE TABLE dbo.InventoryTransactions
(
    TransactionId INT IDENTITY(1,1) PRIMARY KEY,
    InventoryItemId INT NOT NULL,
    Type TINYINT NOT NULL, -- 0=Adjustment,1=Usage,2=Receipt
    Quantity DECIMAL(12,3) NOT NULL,
    Reference NVARCHAR(80) NULL,
    SourceType NVARCHAR(30) NULL,
    SourceId INT NULL,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedByUserId INT NULL,
    CONSTRAINT FK_InventoryTransactions_Item FOREIGN KEY (InventoryItemId) REFERENCES dbo.InventoryItems(InventoryItemId),
    CONSTRAINT FK_InventoryTransactions_User FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(UserId)
);
GO

-- ----------------------
-- Indexes and quick lookups
-- ----------------------
IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Tables_Status' AND object_id = OBJECT_ID('dbo.Tables'))
    CREATE INDEX IX_Tables_Status ON dbo.Tables(Status);
IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Orders_TableId' AND object_id = OBJECT_ID('dbo.Orders'))
    CREATE INDEX IX_Orders_TableId ON dbo.Orders(TableId);
IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Reservations_From' AND object_id = OBJECT_ID('dbo.Reservations'))
    CREATE INDEX IX_Reservations_From ON dbo.Reservations(ReservedFromUtc);
GO

-- ----------------------
-- Seed sample data
-- ----------------------
PRINT 'Seeding sample data...';

-- sample users
INSERT INTO dbo.Users (Username, PasswordHash, DisplayName, Role)
VALUES ('manager', '$2y$12$0T37iQ499Tv.SWXfYN5yGubpbj88Cju9xCe.pNLI.e93DgCexNZMO', 'Admin Manager', 0), ('server', '$2y$12$0T37iQ499Tv.SWXfYN5yGubpbj88Cju9xCe.pNLI.e93DgCexNZMO', 'John Server', 2);

-- sample tables
INSERT INTO dbo.Tables (Code, Location, Capacity, Status)
VALUES
('T1', 'Dining Room', 4, 0),
('T2', 'Dining Room', 4, 2),
('T3', 'Dining Room', 6, 1),
('T4', 'Patio', 2, 0),
('T5', 'Patio', 4, 3),
('T6', 'Patio', 6, 2),
('T7', 'Bar', 2, 0),
('T8', 'Bar', 2, 4);

-- categories and menu items
INSERT INTO dbo.MenuCategories (Name, SortOrder) VALUES ('Mains', 1), ('Sides', 2), ('Drinks', 3);

INSERT INTO dbo.MenuItems (CategoryId, Name, Description, Price, Size, ImageFile)
VALUES
(1, 'Burger', 'Beef burger with lettuce and tomato', 8.50, 'Large', 'burger.png'),
(2, 'Fries', 'Crispy fries', 3.00, 'Regular', 'fries.png'),
(2, 'Salad', 'House salad', 5.00, 'Regular', 'salad.png'),
(3, 'Soda', 'Canned soda', 1.80, 'Can', 'soda.png'),
(1, 'Steak', 'Grilled steak', 15.00, 'Large', 'steak.png'),
(1, 'Pasta', 'Pasta with tomato sauce', 12.00, 'Large', 'pasta.png');
GO

PRINT 'RMS DB schema created and seeded.';

-- seed inventory categories and sample ingredients
INSERT INTO dbo.InventoryCategories (Name) VALUES ('Produce'), ('Meat & Poultry'), ('Pantry');
GO

INSERT INTO dbo.InventorySubcategories (CategoryId, Name) VALUES (1, 'Vegetables'), (1, 'Fruits'), (2, 'Poultry'), (2, 'Beef'), (3, 'Dry Goods');
GO

INSERT INTO dbo.InventoryItems (CategoryId, SubcategoryId, Name, Unit, CurrentStock, ReorderLevel)
VALUES
(1, 1, 'Tomatoes', 'kg', 2.5, 5.0),
(2, 3, 'Chicken Breast', 'kg', 1.2, 3.0),
(3, 5, 'Olive Oil', 'L', 0.5, 2.0);
GO

-- ----------------------
-- Seed inventory transactions (kept) -- adjust SourceId to NULL since receipts/suppliers removed
-- ----------------------
INSERT INTO dbo.InventoryTransactions (InventoryItemId, Type, Quantity, Reference, SourceType, SourceId, CreatedAtUtc, CreatedByUserId)
VALUES
(1, 2, 5.0, 'Initial receipt', 'Receipt', NULL, SYSUTCDATETIME(), NULL),
(2, 2, 10.0, 'Initial receipt', 'Receipt', NULL, SYSUTCDATETIME(), NULL),
(3, 2, 2.0, 'Initial receipt', 'Receipt', NULL, SYSUTCDATETIME(), NULL);
GO
