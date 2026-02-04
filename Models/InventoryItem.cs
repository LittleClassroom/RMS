namespace RMS.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal ReorderLevel { get; set; }
        public string? CategoryName { get; set; }
        public string? SubcategoryName { get; set; }
    }
}
