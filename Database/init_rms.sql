-- RMS database initialization script for SQL Server
-- Comprehensive schema following docs/data-model.md
-- Run in SSMS or sqlcmd with a user that can create databases.

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

-- allocation table for splitting/allocating payments across orders or sessions
IF OBJECT_ID(N'dbo.PaymentAllocations', N'U') IS NOT NULL DROP TABLE dbo.PaymentAllocations;
CREATE TABLE dbo.PaymentAllocations
(
    AllocationId INT IDENTITY(1,1) PRIMARY KEY,
    PaymentId INT NOT NULL,
    OrderId INT NULL,
    SessionId INT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_PaymentAllocations_Payment FOREIGN KEY (PaymentId) REFERENCES dbo.Payments(PaymentId),
    CONSTRAINT FK_PaymentAllocations_Order FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId),
    CONSTRAINT FK_PaymentAllocations_Session FOREIGN KEY (SessionId) REFERENCES dbo.TableSessions(SessionId)
);
GO

-- ----------------------
-- Ingredients
-- ----------------------
IF OBJECT_ID(N'dbo.Ingredients', N'U') IS NOT NULL DROP TABLE dbo.Ingredients;
CREATE TABLE dbo.Ingredients
(
    IngredientId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Unit NVARCHAR(20) NULL,
    CurrentStock DECIMAL(12,3) NOT NULL DEFAULT(0),
    ReorderLevel DECIMAL(12,3) NOT NULL DEFAULT(0)
);
GO

-- ----------------------
-- MenuItemIngredients
-- ----------------------
IF OBJECT_ID(N'dbo.MenuItemIngredients', N'U') IS NOT NULL DROP TABLE dbo.MenuItemIngredients;
CREATE TABLE dbo.MenuItemIngredients
(
    MenuItemId INT NOT NULL,
    IngredientId INT NOT NULL,
    Quantity DECIMAL(10,3) NOT NULL,
    CONSTRAINT PK_MenuItemIngredients PRIMARY KEY (MenuItemId, IngredientId),
    CONSTRAINT FK_MII_MenuItem FOREIGN KEY (MenuItemId) REFERENCES dbo.MenuItems(MenuItemId),
    CONSTRAINT FK_MII_Ingredient FOREIGN KEY (IngredientId) REFERENCES dbo.Ingredients(IngredientId)
);
GO

-- ----------------------
-- InventoryTransactions
-- ----------------------
IF OBJECT_ID(N'dbo.InventoryTransactions', N'U') IS NOT NULL DROP TABLE dbo.InventoryTransactions;
CREATE TABLE dbo.InventoryTransactions
(
    TransactionId INT IDENTITY(1,1) PRIMARY KEY,
    IngredientId INT NOT NULL,
    Type TINYINT NOT NULL, -- 0=Adjustment,1=Usage,2=Receipt
    Quantity DECIMAL(12,3) NOT NULL,
    Reference NVARCHAR(80) NULL,
    SourceType NVARCHAR(30) NULL,
    SourceId INT NULL,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedByUserId INT NULL,
    CONSTRAINT FK_InventoryTransactions_Ingredient FOREIGN KEY (IngredientId) REFERENCES dbo.Ingredients(IngredientId),
    CONSTRAINT FK_InventoryTransactions_User FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(UserId)
);
GO

-- ----------------------
-- Suppliers
-- ----------------------
IF OBJECT_ID(N'dbo.Suppliers', N'U') IS NOT NULL DROP TABLE dbo.Suppliers;
CREATE TABLE dbo.Suppliers
(
    SupplierId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(30) NULL,
    Email NVARCHAR(120) NULL
);
GO

-- ----------------------
-- InventoryReceipts
-- ----------------------
IF OBJECT_ID(N'dbo.InventoryReceipts', N'U') IS NOT NULL DROP TABLE dbo.InventoryReceipts;
CREATE TABLE dbo.InventoryReceipts
(
    ReceiptId INT IDENTITY(1,1) PRIMARY KEY,
    SupplierId INT NULL,
    ReceivedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ReceivedByUserId INT NULL,
    Notes NVARCHAR(200) NULL,
    CONSTRAINT FK_InventoryReceipts_Supplier FOREIGN KEY (SupplierId) REFERENCES dbo.Suppliers(SupplierId),
    CONSTRAINT FK_InventoryReceipts_User FOREIGN KEY (ReceivedByUserId) REFERENCES dbo.Users(UserId)
);
GO

-- ----------------------
-- InventoryReceiptLines
-- ----------------------
IF OBJECT_ID(N'dbo.InventoryReceiptLines', N'U') IS NOT NULL DROP TABLE dbo.InventoryReceiptLines;
CREATE TABLE dbo.InventoryReceiptLines
(
    ReceiptLineId INT IDENTITY(1,1) PRIMARY KEY,
    ReceiptId INT NOT NULL,
    IngredientId INT NOT NULL,
    Quantity DECIMAL(12,3) NOT NULL,
    UnitCost DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_IRL_Receipt FOREIGN KEY (ReceiptId) REFERENCES dbo.InventoryReceipts(ReceiptId) ON DELETE CASCADE,
    CONSTRAINT FK_IRL_Ingredient FOREIGN KEY (IngredientId) REFERENCES dbo.Ingredients(IngredientId)
);
GO

-- ----------------------
-- AuditLog
-- ----------------------
IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NOT NULL DROP TABLE dbo.AuditLog;
CREATE TABLE dbo.AuditLog
(
    AuditId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL,
    Action NVARCHAR(50) NULL,
    Entity NVARCHAR(50) NULL,
    EntityId INT NULL,
    Payload NVARCHAR(MAX) NULL,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_AuditLog_User FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId)
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
VALUES ('manager', NULL, 'Admin Manager', 0), ('chef', NULL, 'Chef Kim', 1), ('server', NULL, 'John Server', 2);

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
