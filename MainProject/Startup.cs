using Database.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MainProject.Models;
using MainProject.Services;
using AutoMapper;
using MainProject.Automapper;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Logging;
using Database.ConfigClass;

using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using Microsoft.IdentityModel.Tokens;
using Database.InfrastructurClass;
using System.Text;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using TMS.Services;

namespace MainProject
{
    public class Startup
    {
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment { get; }

        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllersWithViews();
            services.AddDbContext<BusinessModelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BusinessModel")));

            var mvcBuilder = services.AddControllersWithViews();
#if DEBUG
            mvcBuilder.AddRazorRuntimeCompilation();
#endif
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });
            });
            IdentityModelEventSource.ShowPII = true;


            var systemConfig = Configuration.GetSection("systemConfig").Get<SystemConfig>();
            services.AddSingleton(systemConfig);

            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            var jwtTokenConfig = Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);


            //services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
            //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();


            services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
            services.AddAuthentication(options =>
            {
                // custom scheme defined in .AddPolicyScheme() below
                options.DefaultScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
            }).AddCookie("Cookies", options =>
            {
                options.LoginPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            }).AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtTokenConfig.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
            ValidAudience = jwtTokenConfig.Audience,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    }).AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
    {
        // runs on each request
        options.ForwardDefaultSelector = context =>
        {
            // filter by auth type
            string authorization = context.Request.Headers[HeaderNames.Authorization];
            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                return "Bearer";

            // otherwise always check for cookie auth
            return "Cookies";
        };
    });  


        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BusinessModelContext db)
        {
           
                app.UseDeveloperExceptionPage();
            
            //db.Database.EnsureCreated();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");

                    // Provide client ID, client secret, realm and application name (if need)

                    // Swashbuckle.AspNetCore 4.0.1
                    c.OAuthClientId("swagger-ui");
                c.OAuthClientSecret("swagger-ui-secret");
                c.OAuthRealm("swagger-ui-realm");
                c.OAuthAppName("Swagger UI");

                    // Swashbuckle.AspNetCore 1.1.0
                    // c.ConfigureOAuth2("swagger-ui", "swagger-ui-secret", "swagger-ui-realm", "Swagger UI");
                });

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {

                endpoints.MapAreaControllerRoute(
                    name: "MyAreaMaster",
                    areaName: "Master",
                    pattern: "Master/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "MyAreaTransaction",
                    areaName: "Transaction",
                    pattern: "Transaction/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "MyAreaReport",
                    areaName: "Report",
                    pattern: "Report/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=LoginForm}/{id?}");

            });
        }
    }
}
