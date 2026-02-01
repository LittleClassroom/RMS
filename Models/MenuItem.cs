namespace RMS.Models
{
    public class MenuItemEntity
    {
        public int MenuItemId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Size { get; set; }
        public string? ImageFile { get; set; }
        public bool IsActive { get; set; }
    }
}
