using System;
using System.Linq;
using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Web.Filters;

namespace Web
{
    public class Startup
    {
        public IWebHostEnvironment HostingEnvironment { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureModules(services);
            
            ConfigureCors(services);
            ConfigureMediatr(services);
            ConfigureMvc(services);
            ConfigureSwagger(services);
        }
        
        private void ConfigureModules(IServiceCollection services)
        {
            services.AddInfrastructureModule(Configuration);
            services.AddApplicationModule(Configuration);
        }
        
        private void ConfigureMediatr(IServiceCollection services)
        {
            services.AddScoped<Mediator>();
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        }
        
        private void ConfigureMediatr(IServiceCollection services)
        {
            services.Configure<Rabbi>(configuration.GetSection(JwtOptions.SectionPath));
        }
        
        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Cors", corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }
            ));
        }
        
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
                
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"});
            });
        }
        
        private void ConfigureMvc(IServiceCollection services)
        {
            services
                .AddControllers(x =>
                {
                    x.Filters.Add(new ValidationFilter());
                    x.Filters.Add(new ExceptionFilter(HostingEnvironment));
                })
                .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    }
                )
                .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
                .AddFluentValidation(x 
                    => x.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic)));
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.ApplyMigrations();
        }
    }
}