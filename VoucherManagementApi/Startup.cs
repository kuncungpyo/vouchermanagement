using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Security.Claims;
using VoucherManagementApi.DataAccess.Application;
using VoucherManagementApi.Repository;
using VoucherManagementApi.RepositoryContract;
using VoucherManagementApi.Service;
using VoucherManagementApi.ServiceContract;
using VoucherManagementApip.Infrastructure;

namespace VoucherManagementApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VoucherManagementApi", Version = "v1" });
            });

            var authDomain = this.Configuration["Auth:Domain"];
            var authAudiences = new List<string>() { this.Configuration["Auth:Audiences"] };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Auth0", options =>
            {
                options.Authority = authDomain;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidAudiences = authAudiences
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Auth0")
                    .Build();

                options.AddPolicy("admin,member", new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Auth0")
                    .RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        new List<string>()
                        {
                            "admin",
                            "member"
                        })
                    .Build());

                options.AddPolicy("admin", new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Auth0")
                    .RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", new List<string>() { "admin" })
                    .Build());
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddDbContext<BaseDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IVoucherRepository, VoucherRepository>();
            services.AddTransient<IVoucherRulesRepository, VoucherRulesRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IVoucherRulesService, VoucherRulesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VoucherManagementApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
