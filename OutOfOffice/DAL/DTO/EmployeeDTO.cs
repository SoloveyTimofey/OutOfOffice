using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.DTO
{
    public record EmployeeDTO
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public Subdivision Subdivision { get; set; }
        public Position Position { get; set; }
    }
}
