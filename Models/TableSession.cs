using System;

namespace RMS.Models
{
    public class TableSession
    {
        public int SessionId { get; set; }
        public int TableId { get; set; }
        public int? ReservationId { get; set; }
        public DateTime StartedAtUtc { get; set; }
        public DateTime? EndedAtUtc { get; set; }
        public byte Status { get; set; }
        public int? CreatedByUserId { get; set; }
        public string? Notes { get; set; }
    }
}
