namespace RMS.Models
{
    public class MenuCategory
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
