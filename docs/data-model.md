# RMS Data Model

This document captures the core relational schema for the Restaurant Management System (RMS). It now models dining tables explicitly so front-of-house staff can view availability, seat parties, and tie orders to a table for realistic restaurant flows.

> **Roles** remain `Manager`, `Chef`, `Service`. Stored directly on `Users.Role` (TINYINT). Future versions can externalize permissions.

## Entity Relationship Overview

```
Users (1) ????< Orders (n)
Users (1) ????< Payments (n)
Tables (1) ????< Orders (n)
Tables (1) ????< Reservations (n)
Reservations (1) ??? Orders (0..1)
MenuCategories (1) ????< MenuItems (n)
MenuItems (1) ????< OrderLines (n)
Orders (1) ????< OrderLines (n)
Suppliers (1) ????< InventoryReceipts (n)
Ingredients (1) ????< InventoryTransactions (n)
MenuItems (n) ??< MenuItemIngredients >?? (n) Ingredients
```

## Dining Table Workflow
1. Tables are preconfigured with capacity, location, and status (`Available`, `Reserved`, `Occupied`, `NeedsCleaning`).
2. Host/staff create a `Reservation` (optional) linked to a table and time slot.
3. When guests arrive, staff marks the table `Seated` / `Occupied`, optionally tying the active reservation to a new `Order`.
4. All orders reference `TableId`, enabling quick glances at active tables and open tabs.
5. When the table is paid, status moves to `NeedsCleaning`, then back to `Available` once cleared.

## Tables

### 1. `Users`
| Column          | Type              | Notes |
|----------------|-------------------|-------|
| `UserId`       | INT IDENTITY PK    | |
| `Username`     | NVARCHAR(50)       | Unique |
| `PasswordHash` | NVARCHAR(200)      | BCrypt hash |
| `DisplayName`  | NVARCHAR(100)      | |
| `Role`         | TINYINT            | 0=Manager,1=Chef,2=Service |
| `IsActive`     | BIT                | default 1 |
| `CreatedAtUtc` | DATETIME2          | |

### 2. `Tables`
Represents physical dining tables.
| Column        | Type             | Notes |
|---------------|------------------|-------|
| `TableId`     | INT IDENTITY PK  | |
| `Code`        | NVARCHAR(20)     | Human-readable label (e.g., T1, Patio-3) |
| `Location`    | NVARCHAR(40)     | e.g., Dining Room, Patio |
| `Capacity`    | TINYINT          | Seats |
| `Status`      | TINYINT          | 0=Available,1=Reserved,2=Occupied,3=NeedsCleaning,4=OutOfService |
| `Notes`       | NVARCHAR(200)    | |

### 3. `Reservations`
| Column            | Type            | Notes |
|-------------------|-----------------|-------|
| `ReservationId`   | INT IDENTITY PK | |
| `TableId`         | INT FK -> Tables| |
| `CustomerName`    | NVARCHAR(120)   | |
| `CustomerPhone`   | NVARCHAR(30)    | |
| `PartySize`       | TINYINT         | |
| `ReservedFromUtc` | DATETIME2       | |
| `ReservedToUtc`   | DATETIME2       | |
| `Status`          | TINYINT         | 0=Booked,1=Seated,2=Completed,3=Cancelled,4=NoShow |
| `Notes`           | NVARCHAR(200)   | |

### 4. `MenuCategories`
| Column      | Type            | Notes |
|-------------|-----------------|-------|
| `CategoryId`| INT IDENTITY PK | |
| `Name`      | NVARCHAR(80)    | |
| `SortOrder` | INT             | |

### 5. `MenuItems`
| Column        | Type              | Notes |
|---------------|-------------------|-------|
| `MenuItemId`  | INT IDENTITY PK    | |
| `CategoryId`  | INT FK -> MenuCategories | |
| `Name`        | NVARCHAR(120)      | |
| `Description` | NVARCHAR(400)      | |
| `Price`       | DECIMAL(10,2)      | Current sell price |
| `IsActive`    | BIT                | |

### 6. `Orders`
| Column        | Type              | Notes |
|---------------|-------------------|-------|
| `OrderId`     | INT IDENTITY PK    | |
| `OrderNumber` | NVARCHAR(20)       | Unique per day/shift |
| `TableId`     | INT FK -> Tables   | Mandatory (takeout can map to a special table) |
| `ReservationId` | INT FK -> Reservations NULL | If generated from reservation |
| `CreatedAtUtc`| DATETIME2          | |
| `Status`      | TINYINT            | 0=Open,1=InKitchen,2=Ready,3=Closed,4=Cancelled |
| `CreatedByUserId` | INT FK -> Users | who took order |
| `ClosedByUserId`  | INT FK -> Users NULL | who closed/order paid |
| `GuestCount`  | TINYINT            | optional actual seated count |
| `Notes`       | NVARCHAR(400)      | |

### 7. `OrderLines`
| Column        | Type              | Notes |
|---------------|-------------------|-------|
| `OrderLineId` | INT IDENTITY PK    | |
| `OrderId`     | INT FK -> Orders   | |
| `MenuItemId`  | INT FK -> MenuItems| |
| `Quantity`    | DECIMAL(6,2)       | |
| `UnitPrice`   | DECIMAL(10,2)      | Snapshot price |
| `LineNotes`   | NVARCHAR(200)      | Special instructions |
| `Status`      | TINYINT            | 0=Pending,1=Cooking,2=Ready,3=Served |

### 8. `Payments`
| Column        | Type              | Notes |
|---------------|-------------------|-------|
| `PaymentId`   | INT IDENTITY PK    | |
| `OrderId`     | INT FK -> Orders   | |
| `Amount`      | DECIMAL(10,2)      | |
| `PaymentType` | TINYINT            | 0=Cash,1=Card,2=QR/Wallet |
| `PaidAtUtc`   | DATETIME2          | |
| `ProcessedByUserId` | INT FK -> Users | |
| `Reference`   | NVARCHAR(50)       | Last 4 digits / auth code |

### 9. `Ingredients`
| Column        | Type              | Notes |
|---------------|-------------------|-------|
| `IngredientId`| INT IDENTITY PK    | |
| `Name`        | NVARCHAR(120)      | |
| `Unit`        | NVARCHAR(20)       | e.g. kg, g, L |
| `CurrentStock`| DECIMAL(12,3)      | |
| `ReorderLevel`| DECIMAL(12,3)      | |

### 10. `MenuItemIngredients`
| Column          | Type        |
|-----------------|-------------|
| `MenuItemId`    | INT FK -> MenuItems |
| `IngredientId`  | INT FK -> Ingredients |
| `Quantity`      | DECIMAL(10,3) |
PK: (`MenuItemId`,`IngredientId`)

### 11. `InventoryTransactions`
| Column              | Type              |
|---------------------|-------------------|
| `TransactionId`     | INT IDENTITY PK    |
| `IngredientId`      | INT FK -> Ingredients |
| `Type`              | TINYINT            | 0=Adjustment,1=Usage,2=Receipt |
| `Quantity`          | DECIMAL(12,3)      | Positive=add, Negative=usage |
| `Reference`         | NVARCHAR(80)       | e.g. OrderId/ReceiptId |
| `CreatedAtUtc`      | DATETIME2 |
| `CreatedByUserId`   | INT FK -> Users |

### 12. `Suppliers`
| Column        | Type |
|---------------|------|
| `SupplierId`  | INT IDENTITY PK |
| `Name`        | NVARCHAR(150) |
| `Phone`       | NVARCHAR(30) |
| `Email`       | NVARCHAR(120) |

### 13. `InventoryReceipts`
| Column            | Type |
|-------------------|------|
| `ReceiptId`       | INT IDENTITY PK |
| `SupplierId`      | INT FK -> Suppliers |
| `ReceivedAtUtc`   | DATETIME2 |
| `ReceivedByUserId`| INT FK -> Users |
| `Notes`           | NVARCHAR(200) |

### 14. `InventoryReceiptLines`
| Column            | Type |
|-------------------|------|
| `ReceiptLineId`   | INT IDENTITY PK |
| `ReceiptId`       | INT FK -> InventoryReceipts |
| `IngredientId`    | INT FK -> Ingredients |
| `Quantity`        | DECIMAL(12,3) |
| `UnitCost`        | DECIMAL(10,2) |

### 15. `AuditLog`
| Column      | Type |
|-------------|------|
| `AuditId`   | BIGINT IDENTITY PK |
| `UserId`    | INT FK -> Users |
| `Action`    | NVARCHAR(50) |
| `Entity`    | NVARCHAR(50) |
| `EntityId`  | INT |
| `Payload`   | NVARCHAR(MAX) |
| `CreatedAtUtc` | DATETIME2 |

## Sample Scenario
1. Host sees `Tables` grid, marks `TableId=5` Reserved at 7pm for "Lee" (reservation row inserted).
2. When party arrives, host sets Table status to Occupied, reservation status to Seated, creates an `Order` referencing both.
3. Service staff add order lines; kitchen sees them filtered by Table.
4. After payment, order closes and Table status set to NeedsCleaning, then Available.

## Implementation Guidance
1. Introduce `Role` enum and `CurrentUserRole` helper in code; guard menus (e.g., only Manager sees `usersToolStripMenuItem`).
2. Add data access layer for Tables/Reservations to support host view (e.g., `TableService` with `GetTableStatusSummary`).
3. For take-away orders, create a virtual table (e.g., `TableId=0` labeled "Takeout").
4. When closing an order, update `Tables.Status` accordingly.
5. Create views/stored procedures for table utilization or reservation list.

## Next Steps
- Generate SQL migration scripts using this updated schema.
- Build UI for Table dashboard (status board) and Reservation management.
- Add indexes: `Tables.Status`, `Orders.TableId`, `Reservations.ReservedFromUtc`, etc., for quick lookups.
