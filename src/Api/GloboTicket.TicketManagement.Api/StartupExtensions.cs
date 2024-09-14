using System;
using GloboTicket.TicketManagement.Api.Middleware;
using GloboTicket.TicketManagement.Api.Services;
using GloboTicket.TicketManagement.Application;
using GloboTicket.TicketManagement.Application.Contracts;
using GloboTicket.TicketManagement.Identity;
using GloboTicket.TicketManagement.Infrastructure;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GloboTicket.TicketManagement.Api
{

    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            AddSwagger(builder.Services);
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();

            builder.Services.AddCors(
                options =>
                {
                    options.AddPolicy("Open", builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
                }
            );

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "GloboTicket Ticket Management API");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseCustomExceptionHandler();

            app.UseAuthorization();

            // app.UseRouting();

            app.UseCors("Open");

            app.MapControllers();

            return app;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
              {
                  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                  {
                      Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                      Name = "Authorization",
                      In = ParameterLocation.Header,
                      Type = SecuritySchemeType.ApiKey,
                      Scheme = "Bearer"
                  });

                  c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                      });

                  c.SwaggerDoc("v1", new OpenApiInfo
                  {
                      Version = "v1",
                      Title = "GloboTicket Ticket Management API",

                  });

                  c.OperationFilter<FileResultContentTypeOperationFilter>();
              });
        }

        public static async Task ResetDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            try
            {
                var context = scope.ServiceProvider.GetService<GloboTicketDbContext>();

                if (context != null)
                {
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// Indicates swashbuckle should expose the result of the method as a file in open api (see https://swagger.io/docs/specification/describing-responses/)
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class FileResultContentTypeAttribute : Attribute
        {
            public FileResultContentTypeAttribute(string contentType)
            {
                ContentType = contentType;
            }

            /// <summary>
            /// Content type of the file e.g. image/png
            /// </summary>
            public string ContentType { get; }
        }

        public class FileResultContentTypeOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var requestAttribute = context.MethodInfo.GetCustomAttributes(typeof(FileResultContentTypeAttribute), false)
                    .Cast<FileResultContentTypeAttribute>()
                    .FirstOrDefault();

                if (requestAttribute == null) return;

                operation.Responses.Clear();
                operation.Responses.Add("200", new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
            {
                {
                    requestAttribute.ContentType, new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        }
                    }
                }
            }
                });
            }
        }

    }
}