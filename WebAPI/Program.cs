using FluentValidation;
using MediatR;
using WebAPI.Behaviors;
using WebAPI.Handlers.Document.GetDocument;
using WebAPI.Handlers.Document.SaveDocument;
using WebAPI.Handlers.Document.Validation;
using WebAPI.Domain;
using WebAPI.Repositories;
using WebAPI.Serializers;

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

            builder.Services.AddScoped<IValidator<Document>, DocumentValidator>();
            builder.Services.AddScoped<IValidator<SaveDocumentRequest>, SaveDocumentRequestValidator>();
            builder.Services.AddScoped<IValidator<GetDocumentRequest>, GetDocumentRequestValidator>();

            builder.Services.AddSingleton<IDocumentRepository, JsonDocumentRepository>();
            
            builder.Services.AddScoped<ISerializer, JsonSerializer>();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<Program>();
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(MinimalValidationBehavior<,>));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
