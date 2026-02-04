using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RMS.Models;
using System.Data;

namespace RMS.Data.SqlServer
{
    public class RmsRepository
    {
        private readonly string _conn;

        public RmsRepository(string connectionString)
        {
            _conn = connectionString;
        }

        public IEnumerable<TableInfo> GetTables()
        {
            var list = new List<TableInfo>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("SELECT TableId, Code, Location, Capacity, Status, Notes FROM dbo.Tables", cn);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var t = new TableInfo(rdr.GetString(1), rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2), rdr.GetByte(3), (TableStatus)rdr.GetByte(4))
                {
                    TableId = rdr.GetInt32(0),
                    Notes = rdr.IsDBNull(5) ? null : rdr.GetString(5)
                };
                list.Add(t);
            }
            return list;
        }

        public IEnumerable<(int Id, string Name, decimal Price, string Size, string ImageFile)> GetMenuItems()
        {
            var list = new List<(int, string, decimal, string, string)>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("SELECT MenuItemId, Name, Price, Size, ImageFile FROM dbo.MenuItems WHERE IsActive = 1", cn);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add((rdr.GetInt32(0), rdr.GetString(1), rdr.GetDecimal(2), rdr.IsDBNull(3) ? string.Empty : rdr.GetString(3), rdr.IsDBNull(4) ? string.Empty : rdr.GetString(4)));
            }
            return list;
        }

        public IReadOnlyList<MenuCategory> GetMenuCategories()
        {
            var list = new List<MenuCategory>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("SELECT CategoryId, Name, SortOrder FROM dbo.MenuCategories ORDER BY SortOrder, Name", cn);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new MenuCategory
                {
                    CategoryId = rdr.GetInt32(0),
                    Name = rdr.GetString(1),
                    SortOrder = rdr.GetInt32(2)
                });
            }
            return list;
        }

        public IReadOnlyList<MenuItemEntity> GetMenuItemsByCategory(int? categoryId)
        {
            var list = new List<MenuItemEntity>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("SELECT MenuItemId, CategoryId, Name, Description, Price, Size, ImageFile, IsActive FROM dbo.MenuItems WHERE (@cat IS NULL) OR CategoryId = @cat ORDER BY Name", cn);
            // avoid AddWithValue inference issues by specifying parameter type explicitly
            var p = cmd.Parameters.Add("@cat", SqlDbType.Int);
            if (categoryId.HasValue)
                p.Value = categoryId.Value;
            else
                p.Value = DBNull.Value;

            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new MenuItemEntity
                {
                    MenuItemId = rdr.GetInt32(0),
                    CategoryId = rdr.IsDBNull(1) ? null : rdr.GetInt32(1),
                    Name = rdr.GetString(2),
                    Description = rdr.IsDBNull(3) ? null : rdr.GetString(3),
                    Price = rdr.GetDecimal(4),
                    Size = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                    ImageFile = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                    IsActive = rdr.GetBoolean(7)
                });
            }
            return list;
        }

        public int InsertMenuItem(MenuItemEntity item)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"INSERT INTO dbo.MenuItems (CategoryId, Name, Description, Price, Size, ImageFile, IsActive)
VALUES (@c, @n, @d, @p, @s, @img, @active);
SELECT SCOPE_IDENTITY();", cn);
            cmd.Parameters.AddWithValue("@c", (object?)item.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@n", item.Name);
            cmd.Parameters.AddWithValue("@d", (object?)item.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p", item.Price);
            cmd.Parameters.AddWithValue("@s", (object?)item.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@img", (object?)item.ImageFile ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@active", item.IsActive);
            cn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public void UpdateMenuItem(MenuItemEntity item)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"UPDATE dbo.MenuItems
SET CategoryId = @c,
    Name = @n,
    Description = @d,
    Price = @p,
    Size = @s,
    ImageFile = @img,
    IsActive = @active
WHERE MenuItemId = @id", cn);
            cmd.Parameters.AddWithValue("@id", item.MenuItemId);
            cmd.Parameters.AddWithValue("@c", (object?)item.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@n", item.Name);
            cmd.Parameters.AddWithValue("@d", (object?)item.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p", item.Price);
            cmd.Parameters.AddWithValue("@s", (object?)item.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@img", (object?)item.ImageFile ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@active", item.IsActive);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteMenuItem(int menuItemId)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("DELETE FROM dbo.MenuItems WHERE MenuItemId = @id", cn);
            cmd.Parameters.AddWithValue("@id", menuItemId);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public DashboardSnapshot GetDashboardSnapshot(int recentCount = 8)
        {
            var snapshot = new DashboardSnapshot();
            using var cn = new SqlConnection(_conn);
            var sql = @"
SELECT COUNT(*) AS TotalTables,
       SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END) AS OccupiedTables
FROM dbo.Tables;

SELECT COUNT(*) AS OpenOrders
FROM dbo.Orders
WHERE Status IN (0, 1, 2);

SELECT COUNT(*) AS KitchenQueue
FROM dbo.OrderLines
WHERE Status IN (0, 1);

SELECT COALESCE(SUM(Total), 0) AS TodaysSales,
       COUNT(*) AS OrdersToday,
       COALESCE(SUM(COALESCE(GuestCount, 0)), 0) AS GuestsToday
FROM dbo.Orders
WHERE CAST(CreatedAtUtc AS date) = CAST(SYSUTCDATETIME() AS date);

SELECT AVG(CAST(DATEDIFF(MINUTE, StartedAtUtc, COALESCE(EndedAtUtc, SYSUTCDATETIME())) AS float)) AS AvgMinutes
FROM dbo.TableSessions
WHERE CAST(StartedAtUtc AS date) = CAST(SYSUTCDATETIME() AS date);

SELECT COUNT(*) AS LowStock
FROM dbo.InventoryItems
WHERE CurrentStock <= ReorderLevel;

SELECT TOP (@count)
       o.CreatedAtUtc,
       o.Status,
       o.OrderNumber,
       o.OrderId,
       o.Total,
       t.Code AS TableCode,
       ISNULL(u.DisplayName, u.Username) AS UserDisplay
FROM dbo.Orders o
LEFT JOIN dbo.Tables t ON t.TableId = o.TableId
LEFT JOIN dbo.Users u ON u.UserId = o.CreatedByUserId
ORDER BY o.CreatedAtUtc DESC;";

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@count", recentCount);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            var stats = new DashboardStats();

            if (rdr.Read())
            {
                stats.TotalTables = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                stats.OccupiedTables = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1);
            }

            if (rdr.NextResult() && rdr.Read())
            {
                stats.OpenOrders = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
            }

            if (rdr.NextResult() && rdr.Read())
            {
                stats.KitchenQueue = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
            }

            if (rdr.NextResult() && rdr.Read())
            {
                stats.TodaysSales = rdr.IsDBNull(0) ? 0m : rdr.GetDecimal(0);
                stats.OrdersToday = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1);
                stats.GuestsToday = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2);
            }

            if (rdr.NextResult() && rdr.Read())
            {
                stats.AvgSessionMinutesToday = rdr.IsDBNull(0) ? null : rdr.GetDouble(0);
            }

            if (rdr.NextResult() && rdr.Read())
            {
                stats.LowStockItems = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
            }

            var activities = new List<DashboardActivity>();
            if (rdr.NextResult())
            {
                while (rdr.Read())
                {
                    var status = rdr.GetByte(1);
                    var action = status switch
                    {
                        3 => "Order Closed",
                        2 => "Order Ready",
                        1 => "Order In Kitchen",
                        4 => "Order Cancelled",
                        _ => "Order Created"
                    };

                    var orderNumber = rdr.IsDBNull(2) ? null : rdr.GetString(2);
                    var orderId = rdr.GetInt32(3);
                    var total = rdr.IsDBNull(4) ? 0m : rdr.GetDecimal(4);
                    var tableCode = rdr.IsDBNull(5) ? "N/A" : rdr.GetString(5);
                    var user = rdr.IsDBNull(6) ? "System" : rdr.GetString(6);

                    var details = $"Order {(string.IsNullOrEmpty(orderNumber) ? orderId.ToString() : orderNumber)} - Table {tableCode} - {total:C}";

                    activities.Add(new DashboardActivity
                    {
                        TimestampUtc = rdr.GetDateTime(0),
                        Action = action,
                        Details = details,
                        PerformedBy = user
                    });
                }
            }

            snapshot.Stats = stats;
            snapshot.Activities = activities;
            return snapshot;
        }

        public int CreateOrder(int tableId, decimal subtotal, decimal tax, decimal total, IEnumerable<(int MenuItemId, int Qty, decimal UnitPrice)> lines)
        {
            using var cn = new SqlConnection(_conn);
            cn.Open();
            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(@"INSERT INTO dbo.Orders (TableId, Subtotal, Tax, Total, Status)
OUTPUT INSERTED.OrderId VALUES (@t, @s, @tax, @total, 1)", cn, tx);
                cmd.Parameters.AddWithValue("@t", tableId);
                cmd.Parameters.AddWithValue("@s", subtotal);
                cmd.Parameters.AddWithValue("@tax", tax);
                cmd.Parameters.AddWithValue("@total", total);
                var orderId = (int)cmd.ExecuteScalar();

                foreach (var line in lines)
                {
                    using var cmdLine = new SqlCommand("INSERT INTO dbo.OrderLines (OrderId, MenuItemId, Quantity, UnitPrice) VALUES (@o, @m, @q, @p)", cn, tx);
                    cmdLine.Parameters.AddWithValue("@o", orderId);
                    cmdLine.Parameters.AddWithValue("@m", line.MenuItemId);
                    cmdLine.Parameters.AddWithValue("@q", line.Qty);
                    cmdLine.Parameters.AddWithValue("@p", line.UnitPrice);
                    cmdLine.ExecuteNonQuery();
                }

                tx.Commit();
                return orderId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public OrderSummary? GetOpenOrderForTable(int tableId)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"SELECT TOP 1 o.OrderId, o.TableId, o.Subtotal, o.Tax, o.Total, o.Status, o.IsPaid, o.CreatedAtUtc,
       t.Code, t.Location
FROM dbo.Orders o
LEFT JOIN dbo.Tables t ON t.TableId = o.TableId
WHERE o.TableId = @tableId AND o.IsPaid = 0 AND o.Status IN (0, 1, 2)
ORDER BY o.CreatedAtUtc DESC", cn);
            cmd.Parameters.AddWithValue("@tableId", tableId);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                return null;
            }

            return new OrderSummary
            {
                OrderId = rdr.GetInt32(0),
                TableId = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1),
                Subtotal = rdr.IsDBNull(2) ? 0m : rdr.GetDecimal(2),
                Tax = rdr.IsDBNull(3) ? 0m : rdr.GetDecimal(3),
                Total = rdr.IsDBNull(4) ? 0m : rdr.GetDecimal(4),
                Status = rdr.IsDBNull(5) ? (byte)0 : rdr.GetByte(5),
                IsPaid = !rdr.IsDBNull(6) && rdr.GetBoolean(6),
                CreatedAtUtc = rdr.IsDBNull(7) ? DateTime.UtcNow : rdr.GetDateTime(7),
                TableCode = rdr.IsDBNull(8) ? string.Empty : rdr.GetString(8),
                Location = rdr.IsDBNull(9) ? null : rdr.GetString(9)
            };
        }

        public OrderSummary? GetOrderById(int orderId)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"SELECT o.OrderId, o.TableId, o.Subtotal, o.Tax, o.Total, o.Status, o.IsPaid, o.CreatedAtUtc,
       t.Code, t.Location
FROM dbo.Orders o
LEFT JOIN dbo.Tables t ON t.TableId = o.TableId
WHERE o.OrderId = @orderId", cn);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                return null;
            }

            return new OrderSummary
            {
                OrderId = rdr.GetInt32(0),
                TableId = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1),
                Subtotal = rdr.IsDBNull(2) ? 0m : rdr.GetDecimal(2),
                Tax = rdr.IsDBNull(3) ? 0m : rdr.GetDecimal(3),
                Total = rdr.IsDBNull(4) ? 0m : rdr.GetDecimal(4),
                Status = rdr.IsDBNull(5) ? (byte)0 : rdr.GetByte(5),
                IsPaid = !rdr.IsDBNull(6) && rdr.GetBoolean(6),
                CreatedAtUtc = rdr.IsDBNull(7) ? DateTime.UtcNow : rdr.GetDateTime(7),
                TableCode = rdr.IsDBNull(8) ? string.Empty : rdr.GetString(8),
                Location = rdr.IsDBNull(9) ? null : rdr.GetString(9)
            };
        }

        public IReadOnlyList<ActiveOrderInfo> GetActiveOrders()
        {
            var list = new List<ActiveOrderInfo>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"SELECT o.OrderId, o.TableId, t.Code, t.Location, o.Subtotal, o.Tax, o.Total, o.Status, o.IsPaid, o.CreatedAtUtc
FROM dbo.Orders o
LEFT JOIN dbo.Tables t ON t.TableId = o.TableId
WHERE o.Status IN (0, 1, 2, 3)
ORDER BY CASE WHEN o.IsPaid = 1 THEN 1 ELSE 0 END, o.CreatedAtUtc DESC", cn);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new ActiveOrderInfo
                {
                    OrderId = rdr.GetInt32(0),
                    TableId = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1),
                    TableCode = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    Location = rdr.IsDBNull(3) ? null : rdr.GetString(3),
                    Subtotal = rdr.IsDBNull(4) ? 0m : rdr.GetDecimal(4),
                    Tax = rdr.IsDBNull(5) ? 0m : rdr.GetDecimal(5),
                    Total = rdr.IsDBNull(6) ? 0m : rdr.GetDecimal(6),
                    Status = rdr.IsDBNull(7) ? (byte)0 : rdr.GetByte(7),
                    IsPaid = !rdr.IsDBNull(8) && rdr.GetBoolean(8),
                    CreatedAtUtc = rdr.IsDBNull(9) ? DateTime.UtcNow : rdr.GetDateTime(9)
                });
            }

            return list;
        }

        public void AddOrderLines(int orderId, IEnumerable<(int MenuItemId, int Qty, decimal UnitPrice)> lines)
        {
            using var cn = new SqlConnection(_conn);
            cn.Open();
            using var tx = cn.BeginTransaction();
            try
            {
                foreach (var line in lines)
                {
                    using var cmd = new SqlCommand("INSERT INTO dbo.OrderLines (OrderId, MenuItemId, Quantity, UnitPrice) VALUES (@o, @m, @q, @p)", cn, tx);
                    cmd.Parameters.AddWithValue("@o", orderId);
                    cmd.Parameters.AddWithValue("@m", line.MenuItemId);
                    cmd.Parameters.AddWithValue("@q", line.Qty);
                    cmd.Parameters.AddWithValue("@p", line.UnitPrice);
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void UpdateOrderTotals(int orderId, decimal subtotal, decimal tax, decimal total)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("UPDATE dbo.Orders SET Subtotal = @s, Tax = @t, Total = @total WHERE OrderId = @o", cn);
            cmd.Parameters.AddWithValue("@s", subtotal);
            cmd.Parameters.AddWithValue("@t", tax);
            cmd.Parameters.AddWithValue("@total", total);
            cmd.Parameters.AddWithValue("@o", orderId);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateTableStatus(int tableId, TableStatus status)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("UPDATE dbo.Tables SET Status = @status WHERE TableId = @tableId", cn);
            cmd.Parameters.AddWithValue("@status", (byte)status);
            cmd.Parameters.AddWithValue("@tableId", tableId);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void MarkOrderPaid(int orderId)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("UPDATE dbo.Orders SET Status = 3, IsPaid = 1 WHERE OrderId = @orderId", cn);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // Records a payment against an order or session and returns the inserted PaymentId
        public int CreatePayment(int? orderId, int? sessionId, decimal amount, byte paymentType = 0, int? processedByUserId = null, string? reference = null, bool isRefund = false, int? relatedPaymentId = null)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"INSERT INTO dbo.Payments (OrderId, SessionId, Amount, PaymentType, PaidAtUtc, ProcessedByUserId, Reference, IsRefund, RelatedPaymentId)
                                            OUTPUT INSERTED.PaymentId
                                            VALUES (@orderId, @sessionId, @amount, @ptype, SYSUTCDATETIME(), @procBy, @ref, @isRefund, @related)", cn);

            cmd.Parameters.AddWithValue("@orderId", (object?)orderId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@sessionId", (object?)sessionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@ptype", paymentType);
            cmd.Parameters.AddWithValue("@procBy", (object?)processedByUserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ref", (object?)reference ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@isRefund", isRefund);
            cmd.Parameters.AddWithValue("@related", (object?)relatedPaymentId ?? DBNull.Value);

            cn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public IReadOnlyList<OrderLineDetail> GetOrderLines(int orderId)
        {
            var list = new List<OrderLineDetail>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"SELECT ol.OrderLineId, ol.MenuItemId, COALESCE(mi.Name, 'Item #' + CAST(ol.MenuItemId AS varchar(12))) AS Name,
       ol.Quantity, ol.UnitPrice
FROM dbo.OrderLines ol
LEFT JOIN dbo.MenuItems mi ON mi.MenuItemId = ol.MenuItemId
WHERE ol.OrderId = @orderId
ORDER BY ol.OrderLineId", cn);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var quantityValue = rdr.GetValue(3);
                var quantity = quantityValue switch
                {
                    int i => i,
                    short s => s,
                    long l => (int)l,
                    decimal d => (int)d,
                    double dbl => (int)dbl,
                    _ => Convert.ToInt32(quantityValue)
                };

                list.Add(new OrderLineDetail
                {
                    OrderLineId = rdr.GetInt32(0),
                    MenuItemId = rdr.GetInt32(1),
                    Name = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    Quantity = quantity,
                    UnitPrice = rdr.GetDecimal(4)
                });
            }
            return list;
        }

        public int InsertTable(TableInfo table)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"INSERT INTO dbo.Tables (Code, Location, Capacity, Status, Notes)
OUTPUT INSERTED.TableId VALUES (@code, @loc, @cap, @status, @notes)", cn);
            cmd.Parameters.AddWithValue("@code", table.Code);
            cmd.Parameters.AddWithValue("@loc", (object?)table.Location ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cap", table.Capacity);
            cmd.Parameters.AddWithValue("@status", (byte)table.Status);
            cmd.Parameters.AddWithValue("@notes", (object?)table.Notes ?? DBNull.Value);
            cn.Open();
            var id = (int)cmd.ExecuteScalar();
            return id;
        }

        public void UpdateTable(TableInfo table)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"UPDATE dbo.Tables
SET Code = @code,
    Location = @loc,
    Capacity = @cap,
    Status = @status,
    Notes = @notes
WHERE TableId = @id", cn);
            cmd.Parameters.AddWithValue("@id", table.TableId);
            cmd.Parameters.AddWithValue("@code", table.Code);
            cmd.Parameters.AddWithValue("@loc", (object?)table.Location ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cap", table.Capacity);
            cmd.Parameters.AddWithValue("@status", (byte)table.Status);
            cmd.Parameters.AddWithValue("@notes", (object?)table.Notes ?? DBNull.Value);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteTable(int tableId)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("DELETE FROM dbo.Tables WHERE TableId = @id", cn);
            cmd.Parameters.AddWithValue("@id", tableId);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public IReadOnlyList<InventoryCategory> GetInventoryCategories()
        {
            var list = new List<InventoryCategory>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("SELECT CategoryId, Name FROM dbo.InventoryCategories ORDER BY Name", cn);
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new InventoryCategory
                {
                    CategoryId = rdr.GetInt32(0),
                    Name = rdr.GetString(1)
                });
            }
            return list;
        }

        public IReadOnlyList<InventorySubcategory> GetInventorySubcategories(int? categoryId = null)
        {
            var list = new List<InventorySubcategory>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"SELECT SubcategoryId, CategoryId, Name
FROM dbo.InventorySubcategories
WHERE (@cat IS NULL OR CategoryId = @cat)
ORDER BY Name", cn);
            var pCat = cmd.Parameters.Add("@cat", SqlDbType.Int);
            pCat.Value = categoryId.HasValue ? categoryId.Value : DBNull.Value;
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new InventorySubcategory
                {
                    SubcategoryId = rdr.GetInt32(0),
                    CategoryId = rdr.GetInt32(1),
                    Name = rdr.GetString(2)
                });
            }
            return list;
        }

        public IReadOnlyList<InventoryItem> GetInventoryItems(int? categoryId = null, int? subcategoryId = null)
        {
            var list = new List<InventoryItem>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"SELECT i.InventoryItemId,
       i.CategoryId,
       c.Name AS CategoryName,
       i.SubcategoryId,
       sc.Name AS SubcategoryName,
       i.Name,
       i.Unit,
       i.CurrentStock,
       i.ReorderLevel
FROM dbo.InventoryItems i
LEFT JOIN dbo.InventoryCategories c ON c.CategoryId = i.CategoryId
LEFT JOIN dbo.InventorySubcategories sc ON sc.SubcategoryId = i.SubcategoryId
WHERE (@cat IS NULL OR i.CategoryId = @cat)
  AND (@sub IS NULL OR i.SubcategoryId = @sub)
ORDER BY i.Name", cn);
            cmd.Parameters.Add("@cat", SqlDbType.Int).Value = categoryId.HasValue ? categoryId.Value : DBNull.Value;
            cmd.Parameters.Add("@sub", SqlDbType.Int).Value = subcategoryId.HasValue ? subcategoryId.Value : DBNull.Value;
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new InventoryItem
                {
                    InventoryItemId = rdr.GetInt32(0),
                    CategoryId = rdr.IsDBNull(1) ? null : rdr.GetInt32(1),
                    CategoryName = rdr.IsDBNull(2) ? null : rdr.GetString(2),
                    SubcategoryId = rdr.IsDBNull(3) ? null : rdr.GetInt32(3),
                    SubcategoryName = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                    Name = rdr.IsDBNull(5) ? string.Empty : rdr.GetString(5),
                    Unit = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                    CurrentStock = rdr.IsDBNull(7) ? 0m : rdr.GetDecimal(7),
                    ReorderLevel = rdr.IsDBNull(8) ? 0m : rdr.GetDecimal(8)
                });
            }
            return list;
        }

        public int InsertInventoryItem(InventoryItem item)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"INSERT INTO dbo.InventoryItems (CategoryId, SubcategoryId, Name, Unit, CurrentStock, ReorderLevel)
OUTPUT INSERTED.InventoryItemId
VALUES (@cat, @sub, @name, @unit, @stock, @reorder)", cn);
            cmd.Parameters.AddWithValue("@cat", (object?)item.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@sub", (object?)item.SubcategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@name", item.Name);
            cmd.Parameters.AddWithValue("@unit", (object?)item.Unit ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@stock", item.CurrentStock);
            cmd.Parameters.AddWithValue("@reorder", item.ReorderLevel);
            cn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public IReadOnlyList<InventoryTransaction> GetInventoryTransactions(int? inventoryItemId = null)
        {
            var list = new List<InventoryTransaction>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"SELECT TransactionId, InventoryItemId, Type, Quantity, Reference, SourceType, SourceId, CreatedAtUtc, CreatedByUserId
FROM dbo.InventoryTransactions
WHERE (@item IS NULL OR InventoryItemId = @item)
ORDER BY CreatedAtUtc DESC", cn);
            var p = cmd.Parameters.Add("@item", SqlDbType.Int);
            p.Value = inventoryItemId.HasValue ? inventoryItemId.Value : DBNull.Value;
            cn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new InventoryTransaction
                {
                    TransactionId = rdr.GetInt32(0),
                    InventoryItemId = rdr.GetInt32(1),
                    Type = rdr.GetByte(2),
                    Quantity = rdr.GetDecimal(3),
                    Reference = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                    SourceType = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                    SourceId = rdr.IsDBNull(6) ? null : (int?)rdr.GetInt32(6),
                    CreatedAtUtc = rdr.IsDBNull(7) ? DateTime.UtcNow : rdr.GetDateTime(7),
                    CreatedByUserId = rdr.IsDBNull(8) ? null : (int?)rdr.GetInt32(8)
                });
            }
            return list;
        }

        public int CreateInventoryTransaction(int inventoryItemId, byte type, decimal quantity, string? reference = null, string? sourceType = null, int? sourceId = null, int? createdByUserId = null)
        {
            using var cn = new SqlConnection(_conn);
            cn.Open();
            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(@"INSERT INTO dbo.InventoryTransactions (InventoryItemId, Type, Quantity, Reference, SourceType, SourceId, CreatedAtUtc, CreatedByUserId)
OUTPUT INSERTED.TransactionId
VALUES (@item, @type, @qty, @ref, @stype, @sid, SYSUTCDATETIME(), @createdBy)", cn, tx);
                cmd.Parameters.AddWithValue("@item", inventoryItemId);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@qty", quantity);
                cmd.Parameters.AddWithValue("@ref", (object?)reference ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@stype", (object?)sourceType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sid", (object?)sourceId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)createdByUserId ?? DBNull.Value);

                var idObj = cmd.ExecuteScalar();
                var txId = Convert.ToInt32(idObj);

                decimal delta = type == 1 ? -quantity : quantity;

                using var upd = new SqlCommand("UPDATE dbo.InventoryItems SET CurrentStock = ISNULL(CurrentStock, 0) + @delta WHERE InventoryItemId = @item", cn, tx);
                upd.Parameters.AddWithValue("@delta", delta);
                upd.Parameters.AddWithValue("@item", inventoryItemId);
                upd.ExecuteNonQuery();

                tx.Commit();
                return txId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}
