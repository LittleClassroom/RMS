namespace RMS.Models
{
    public class KitchenLineInfo
    {
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public string? TableCode { get; set; }
        public System.DateTime OrderCreatedAtUtc { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public byte LineStatus { get; set; }
        public byte OrderStatus { get; set; }
    }
}
