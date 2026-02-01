namespace RMS.Models
{
    public enum TableStatus
    {
        Available,
        Reserved,
        Occupied,
        NeedsCleaning,
        OutOfService
    }

    public class TableInfo
    {
        public int TableId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
        public string? Notes { get; set; }
        public int SeatedCount { get; set; }

        public TableInfo() { }

        public TableInfo(string code, string location, int capacity, TableStatus status)
        {
            Code = code;
            Location = location;
            Capacity = capacity;
            Status = status;
        }
    }
}
