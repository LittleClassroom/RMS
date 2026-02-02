using System;

namespace RMS.Models
{
    public class OrderSummary
    {
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public string TableCode { get; set; } = string.Empty;
        public string? Location { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public byte Status { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class OrderLineDetail
    {
        public int OrderLineId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ActiveOrderInfo
    {
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public string TableCode { get; set; } = string.Empty;
        public string? Location { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public byte Status { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
