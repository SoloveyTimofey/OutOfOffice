namespace OutOfOffice.DAL.Models
{
    public abstract class EmployeeBase
    {
        public long Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public Subdivision Subdivition { get; set; }
        public Position Position { get; set; }
        public bool IsActive { get; set; }

        public long OutOfOfficeBalance { get; set; } = 7;

        public byte[]? Photo { get; set; }
    }

    public enum Subdivision
    {
        SoftwareDevelopment,
        QA,
        MarketingAndSales,
        DataAnalytics
    }

    public enum Position
    {
        Junior,
        Middle,
        Senior,
        Other
    }
}
