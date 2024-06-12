using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOffice;
using OutOfOffice.DAL.Contexts;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;
using OutOfOffice.DAL.Repositories.Realisations;
using OutOfOffice.Services.Facade;
using OutOfOffice.Services.Image;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opts =>
{
    opts.IdleTimeout = TimeSpan.FromHours(12);
    opts.Cookie.IsEssential = true;
    opts.Cookie.HttpOnly = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserGetRepository, UserRepository>();
builder.Services.AddScoped<IUserActionRepository, UserRepository>();
builder.Services.AddScoped<IRequestActionRepository, RequestRepository>();
builder.Services.AddScoped<IRequestGetRepository, RequestRepository>();
builder.Services.AddScoped<IProjectGetRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectActionRepository, ProjectRepository>();

builder.Services.AddScoped<ICreateApprivalRequestFacade, CreateApprovalRequestFacade>();

builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddDbContext<OutOfOfficeDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:OutOfOfficeConnection"]);
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<OutOfOfficeDbContext>();

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireDigit = false;
    opts.Password.RequireUppercase = false;
    opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
});

var app = builder.Build();

app.UseMiddleware<SeedDataMiddleware>();

if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Account/AccessDenied");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
