using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Contexts;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;

namespace OutOfOffice.DAL.Repositories.Realisations
{
    public class UserRepository : IUserGetRepository, IUserActionRepository
    {
        private readonly OutOfOfficeDbContext context;
        public UserRepository(OutOfOfficeDbContext context) 
        {
            this.context = context;
        }

        #region Get
        public IQueryable<HRManager> GetAllHRsAsync()
        {
            return context.HRManagers;
        }
        public async Task<EmployeeBase> GetEmployeeByIdAsync(long id)
        {
            return await context.AllEmployees.FindAsync(id) ?? throw new ArgumentException(nameof(id));
        }
        public IQueryable<EmployeeBase> GetAllEmployees()
        {
            return context.Employees;
        }
        public IQueryable<Employee> GetHREmployees(long hrID)
        {
            return context.Employees.Where(e=>e.HRManagerId == hrID);
        }
        public IQueryable<EmployeeBase> GetEmployeeByIdAsync(List<long> IDs)
        {
            return context.Employees.Where(e => IDs.Any(id => id == e.Id));
        }
        #endregion

        #region Action
        public async Task<EmployeeBase> CreateNewEmployeeAsync(EmployeeBase employee)
        {
            if (employee is Employee emp)
            {
                await context.Employees.AddAsync(emp);
                await context.SaveChangesAsync();
                return context.Entry(emp).Entity;
            }
            else if (employee is ProjectManager pm)
            {
                await context.ProjectManagers.AddAsync(pm);
                await context.SaveChangesAsync();
                return context.Entry(pm).Entity;
            }
            else if (employee is HRManager hr)
            {
                await context.HRManagers.AddAsync(hr);
                await context.SaveChangesAsync();
                return context.Entry(hr).Entity;
            }

            throw new ArgumentException(nameof(employee));

        }

        public async Task DeleteEmployeeAsync(long id)
        {
            Employee emp = await context.Employees.FirstOrDefaultAsync(e => e.Id == id) ?? throw new ArgumentException(nameof(id));
            context.Employees.Remove(emp);
            await context.SaveChangesAsync();
        }

        public async Task ChangeOutOfOfficeBalanceAsync(long outOfOfficeBalance, long empId)
        {
            EmployeeBase emp = await context.AllEmployees.FirstOrDefaultAsync(e=>e.Id== empId)?? throw new ArgumentException(nameof(empId));

            emp.OutOfOfficeBalance = outOfOfficeBalance;

            await context.SaveChangesAsync();
        }

        public async Task ChangeEmployeeActivity(bool activity, long empId)
        {
            EmployeeBase emp = await context.AllEmployees.FirstOrDefaultAsync(e => e.Id == empId) ?? throw new ArgumentException(nameof(empId));

            emp.IsActive = activity;

            await context.SaveChangesAsync();   
        }
        #endregion
    }
}
