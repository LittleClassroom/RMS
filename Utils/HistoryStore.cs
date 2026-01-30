using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace RMS.Utils
{
    public class ConnectionEntry
    {
        public string DataSource { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public bool Integrated { get; set; }
        public DateTime TimestampUtc { get; set; }
    }

    public static class HistoryStore
    {
        private static readonly string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CSharpAssignment");
        private static readonly string HistoryFile = Path.Combine(AppFolder, "db_history.json");

        public static List<ConnectionEntry> LoadHistory()
        {
            try
            {
                if (!File.Exists(HistoryFile)) return new List<ConnectionEntry>();
                var txt = File.ReadAllText(HistoryFile);
                var list = JsonSerializer.Deserialize<List<ConnectionEntry>>(txt);
                return list ?? new List<ConnectionEntry>();
            }
            catch
            {
                return new List<ConnectionEntry>();
            }
        }

        public static void Append(ConnectionEntry entry)
        {
            if (entry == null) return;
            Directory.CreateDirectory(AppFolder);
            try
            {
                var list = LoadHistory();
                list.RemoveAll(x => string.Equals(x.DataSource, entry.DataSource, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(x.Database, entry.Database, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(x.UserID, entry.UserID, StringComparison.OrdinalIgnoreCase)
                    && x.Integrated == entry.Integrated);
                entry.TimestampUtc = DateTime.UtcNow;
                list.Insert(0, entry);
                list = list.Take(20).ToList();
                File.WriteAllText(HistoryFile, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch
            {
            }
        }
    }
}
