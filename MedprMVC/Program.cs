using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using Microsoft.EntityFrameworkCore;
using MedprDB;
using MedprDB.Entities;
using MedprBusiness;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using MedprRepositories;
using MedprAbstractions.Repositories;
using MedprAbstractions;
using MedprDataRepositories;
using MedprMVC.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using MedprBusiness.ServiceImplimentations.Repository;

namespace MedprMVC;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, lc) =>
            lc.WriteTo.File(
                @"C:\Skyrim\Code\ASPNET\Medpr\Logs\data.log",
                LogEventLevel.Information,
                retainedFileCountLimit: 20,
                rollingInterval: RollingInterval.Hour)
                .WriteTo.Console(LogEventLevel.Verbose));

        builder.Services.AddControllersWithViews();

        var connectionString = builder.Configuration.GetConnectionString("Default");

        builder.Services.AddDbContext<MedprDBContext>(
            optionsBuilder => optionsBuilder.UseSqlServer(connectionString));

        var connectionStringIdentity = builder.Configuration.GetConnectionString("Identity");

        builder.Services.AddDbContext<IdentityDBContext>(
            optionsBuilder => optionsBuilder.UseSqlServer(connectionStringIdentity));

        builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDBContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole",
                 policy => policy.RequireRole("Admin"));
            options.AddPolicy("RequireDefaultRole",
                 policy => policy.RequireRole("Default"));
        });

        builder.Services.ConfigureApplicationCookie(opts =>
        {
            opts.LoginPath = new PathString("/Home/Login");
            opts.LogoutPath = new PathString("/Home/Login");
            opts.AccessDeniedPath = new PathString("/Home/Denied");
        });

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IDrugService, DrugServiceRepository>();
        builder.Services.AddScoped<IDoctorService, DoctorServiceRepository>();
        builder.Services.AddScoped<IFamilyService, FamilyService>();
        builder.Services.AddScoped<IVaccineService, VaccineServiceRepository>();
        builder.Services.AddScoped<IVaccinationService, VaccinationService>();
        builder.Services.AddScoped<IAppointmentService, AppointmentService>();
        builder.Services.AddScoped<IFamilyMemberService, FamilyMemberService>();
        builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

        builder.Services.AddScoped<IRepository<User>, Repository<User>>();
        builder.Services.AddScoped<IRepository<Drug>, Repository<Drug>>();
        builder.Services.AddScoped<IRepository<Doctor>, Repository<Doctor>>();
        builder.Services.AddScoped<IRepository<Family>, Repository<Family>>();
        builder.Services.AddScoped<IRepository<Vaccine>, Repository<Vaccine>>();
        builder.Services.AddScoped<IRepository<Vaccination>, Repository<Vaccination>>();
        builder.Services.AddScoped<IRepository<Appointment>, Repository<Appointment>>();
        builder.Services.AddScoped<IRepository<FamilyMember>, Repository<FamilyMember>>();
        builder.Services.AddScoped<IRepository<Prescription>, Repository<Prescription>>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Login}/{id?}");

        app.Run();
    }
}