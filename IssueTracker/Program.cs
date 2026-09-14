
using IssueTracker.ExceptionHandling;
using IssueTracker.Infrastructure;
using IssueTracker.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IIssueRepository, IssueRepository>();
            builder.Services.AddScoped<IIssueService, IssueService>();
            //builder.Services.AddScoped<AppDbContext>();
            builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            builder.Services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddExceptionHandler<MyExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Type T can be inferred!
            //static T Echo<T>(T value)
            //{
            //    return value;
            //}

            //var number = Echo(42);
            //var text = Echo("hello");
            //Console.WriteLine($"number is {number.GetType().Name} and text is {text.GetType().Name}");

            app.Run();
        }
    }
}
