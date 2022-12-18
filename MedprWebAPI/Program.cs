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
using MedprBusiness.ServiceImplimentations.CQS;
using MediatR;
using System.Reflection;
using MedprBusiness.ServiceImplimentations.Repository;
using MedprCQS.Queries;
using MedprCQS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AspNetSample.WebAPI.Utils;
using MedprBusiness.ServiceImplimentations.Cqs;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.SqlServer;
using MedprWebAPI.Utils.Notifications;

namespace MedprWebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR();

        builder.Host.UseSerilog((ctx, lc) =>
        lc.WriteTo.File(
            builder.Configuration["Serilog"],
            LogEventLevel.Information,
            retainedFileCountLimit: 20,
            rollingInterval: RollingInterval.Hour)
            .WriteTo.Console(LogEventLevel.Verbose));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("medpr", policyBuilder =>
            {
                policyBuilder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        // Main database
        builder.Services.AddDbContext<MedprDBContext>(
            optionsBuilder => optionsBuilder.UseSqlServer(
                builder.Configuration.GetConnectionString("Default")));
        // Identity database
        builder.Services.AddDbContext<IdentityDBContext>(
            optionsBuilder => optionsBuilder.UseSqlServer(
                builder.Configuration.GetConnectionString("Identity")));

        builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDBContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = builder.Configuration["Token:Project"],
                ValidAudience = builder.Configuration["Token:Project"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:JwtSecret"])),
                ClockSkew = TimeSpan.Zero
            };
        });

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

        // Add Hangfire services.
        builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("Default"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        // Add the processing server as IHostedService
        builder.Services.AddHangfireServer();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<IUserService, UserServiceCqs>();
        builder.Services.AddScoped<IDrugService, DrugServiceCqs>();
        builder.Services.AddScoped<IDoctorService, DoctorServiceCqs>();
        builder.Services.AddScoped<IFamilyService, FamilyServiceCqs>();
        builder.Services.AddScoped<IVaccineService, VaccineServiceCqs>();
        builder.Services.AddScoped<IVaccinationService, VaccinationServiceCqs>();
        builder.Services.AddScoped<IAppointmentService, AppointmentServiceCqs>();
        builder.Services.AddScoped<IFamilyMemberService, FamilyMemberServiceCqs>();
        builder.Services.AddScoped<IPrescriptionService, PrescriptionServiceCqs>();
        builder.Services.AddScoped<IFeedService, FeedServiceCqs>();

        builder.Services.AddScoped<IJwtUtil, JwtUtilSha256>();
        builder.Services.AddScoped<INotificationService, NotificationService>();

        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(typeof(ClassToAddMediator).Assembly);


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(builder.Configuration["Documentation"]);
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Example: \"Bearer 1safsfsdfdfd\"",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
            });
        });

        builder.Services.AddHttpClient<OpenFDAService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseHangfireDashboard();

        app.UseHttpsRedirection();
        app.UseCors("medpr");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<EventNotificationHub>("/notify");

        app.MapControllers();

        app.Run();
    }
}