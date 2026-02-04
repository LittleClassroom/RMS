using System;

namespace RMS.Models
{
    public class InventoryTransaction
    {
        public int TransactionId { get; set; }
        public int InventoryItemId { get; set; }
        public byte Type { get; set; }
        public decimal Quantity { get; set; }
        public string? Reference { get; set; }
        public string? SourceType { get; set; }
        public int? SourceId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
