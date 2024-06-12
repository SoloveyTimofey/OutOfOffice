using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Contexts
{
    public class OutOfOfficeDbContext : IdentityDbContext<ApplicationUser>
    {
        public OutOfOfficeDbContext(DbContextOptions<OutOfOfficeDbContext> options) : base(options)
        {
        }

        public DbSet<EmployeeBase> AllEmployees { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HRManager> HRManagers { get; set; }
        public DbSet<ProjectManager> ProjectManagers { get; set; }

        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<EmployeeProjectJunction> EmployeeProjectJunctions { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeBase>().UseTphMappingStrategy();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.HRManager)
                .WithMany(hr => hr.Employees)
                .HasForeignKey(e => e.HRManagerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProjectManager>()
                .HasOne(pm => pm.HRManager)
                .WithMany(hr => hr.ProjectManagers)
                .HasForeignKey(pm => pm.HRManagerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr=>lr.ApprovalRequest)
                .WithOne(ar=>ar.LeaveRequest)
                .HasForeignKey<ApprovalRequest>(ar=>ar.LeaveRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Employees)
                .WithMany(e => e.Projects)
                .UsingEntity<EmployeeProjectJunction>(
                 j => j
                    .HasOne(epj => epj.Employee)
                    .WithMany(e => e.EmployeeProjectJunctions)
                    .HasForeignKey(epj => epj.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict),
                 j => j
                .HasOne(epj => epj.Project)
                .WithMany(p => p.EmployeeProjectJunctions)
                .HasForeignKey(epj => epj.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
        );

            base.OnModelCreating(modelBuilder);
        }
    }
}
