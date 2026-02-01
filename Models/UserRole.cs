namespace RMS.Models
{
    /// <summary>
    /// User roles matching the data model (Users.Role TINYINT)
    /// </summary>
    public enum UserRole
    {
        Manager = 0,
        Chef = 1,
        Service = 2  // Cashier, Server, Host
    }
}
