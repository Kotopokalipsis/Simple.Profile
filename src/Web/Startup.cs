using System;
using System.Linq;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("ProfileDB"))
            );
            
            services.AddCors(options => options.AddPolicy("Cors", corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }
            ));

            // services.AddScoped<IRepository<UserRefreshToken>, Repository<UserRefreshToken>>();
            // services.AddScoped<IRepository<RefreshTokenBlacklist>, Repository<RefreshTokenBlacklist>>();

            services.AddScoped<Mediator>();
            
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            
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
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        }
    }
}