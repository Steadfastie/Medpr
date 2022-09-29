using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using Microsoft.EntityFrameworkCore;
using MedprDB;
using MedprDB.Entities;
using MedprBusiness;
using MedprBusiness.ServiceImplementations;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using MedprRepositories;
using MedprAbstractions.Repositories;
using MedprAbstractions;
using MedprDataRepositories;

namespace MedprMVC
{
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

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IDrugService, DrugService>();
            builder.Services.AddScoped<IRepository<Drug>, Repository<Drug>>();
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}