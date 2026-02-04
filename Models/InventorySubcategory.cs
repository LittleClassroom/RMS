namespace RMS.Models
{
    public class InventorySubcategory
    {
        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
