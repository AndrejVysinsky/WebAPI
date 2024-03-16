using FluentValidation;
using MediatR;
using WebAPI.Behaviors;
using WebAPI.Handlers.Document.GetDocument;
using WebAPI.Handlers.Document.SaveDocument;
using WebAPI.Repositories;
using WebAPI.Serializers;
using WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Handlers.Document.UpdateDocument;
using WebAPI.Processors;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<JsonDocumentSettings>(builder.Configuration.GetSection(JsonDocumentSettings.Key));
            
            var connectionString = builder.Configuration.GetConnectionString("AzureSqlConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IDocumentRepository, DbDocumentRepository>();

            builder.Services.AddScoped<IValidator<SaveDocumentRequest>, SaveDocumentRequestValidator>();
            builder.Services.AddScoped<IValidator<UpdateDocumentRequest>, UpdateDocumentRequestValidator>();
            builder.Services.AddScoped<IValidator<GetDocumentRequest>, GetDocumentRequestValidator>();
            
            builder.Services.AddScoped<ISerializer, JsonSerializer>();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<Program>();
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            var app = builder.Build();

            app.UseSwagger();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ValidationMappingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
