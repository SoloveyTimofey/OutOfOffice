using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Contexts;
using OutOfOffice.DAL.Models;

namespace OutOfOffice
{
    public class SeedDataMiddleware
    {
        private readonly RequestDelegate next;
        public SeedDataMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, OutOfOfficeDbContext dbContext)
        {
            if (dbContext.Database.GetPendingMigrations().Count() == 0)
            {
                if (dbContext.Roles.Count() == 0)
                {
                    foreach (var role in GetRoles())
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
                if (dbContext.Users.Count() == 0)
                {
                    await SeedInitialEmployeesAndTheirAccounts(dbContext, userManager, roleManager);
                }
            }

            await next(httpContext);
        }

        private List<IdentityRole> GetRoles()=> new List<IdentityRole>
        {
            new IdentityRole{Name="Employee"},
            new IdentityRole{Name="HR Manager"},
            new IdentityRole{Name="Project Manager"},
            new IdentityRole{Name ="Administrator"}
        };

        private async Task SeedInitialEmployeesAndTheirAccounts(OutOfOfficeDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            HRManager initialHR = new HRManager()
            {
                FullName = "Yuliia Khomiakova",
                IsActive = true,
                OutOfOfficeBalance = 7,
                Position = Position.Other,
                Subdivition = Subdivision.SoftwareDevelopment
            };

            await context.AddAsync(initialHR);
            await context.SaveChangesAsync();
            initialHR = context.Entry(initialHR).Entity;

            ApplicationUser initialHRIdentity = new ApplicationUser
            {
                Email = "yulkh@smart-it.com",
                EmployeeId = initialHR.Id,
                UserName = initialHR.FullName
            };

            var hrPswrdResult = await userManager.CreateAsync(initialHRIdentity, "initialHrPassword");
            if (!hrPswrdResult.Succeeded)
            {
                throw new ArgumentException(nameof(hrPswrdResult));
            }

            var addToRoleResult = await userManager.AddToRoleAsync(initialHRIdentity, "HR Manager");
            if (!addToRoleResult.Succeeded)
            {
                throw new ArgumentException(nameof(addToRoleResult));
            }
        }
    }
}
