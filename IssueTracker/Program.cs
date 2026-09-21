using IssueTracker.ExceptionHandling;
using IssueTracker.Infrastructure;
using IssueTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("IssueWrite", policy =>
                    policy.RequireClaim("scope", "issues.write"));
                options.AddPolicy("IssueRead", policy =>
                    policy.RequireClaim("scope", "issues.read"));
                options.AddPolicy("IssueDelete", policy =>
                    policy.RequireClaim("scope", "issues.delete"));
            });

            builder.Services.AddScoped<IIssueRepository, IssueRepository>();
            builder.Services.AddScoped<IIssueService, IssueService>();
            builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
