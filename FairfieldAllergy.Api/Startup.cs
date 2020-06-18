using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FairfieldAllergy.Api.Security;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FairfieldAllergy.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = (IConfigurationRoot)configuration;
        //}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string [] listOfOrigions = new string[7];
            listOfOrigions[0] = "http://192.168.1.110:8080";
            listOfOrigions[1] = "http://localhost:8082";
            listOfOrigions[2] = "http://localhost:8081";
            listOfOrigions[3] = "http://localhost:8080";
            listOfOrigions[4] = "http://localhost:3000";
            listOfOrigions[5] = "http://localhost:3001";
            listOfOrigions[6] = "http://192.168.1.110:3001";

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(listOfOrigions)
                .AllowAnyHeader());
                //        .AllowCredentials()); ; ;
            });

            services.AddIdentity<FairfieldIdentityUser, IdentityRole>(options =>
            {
                //options.Password.
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                //options.Password.RequiredUniqueChars = 3;

                options.SignIn.RequireConfirmedEmail = false;

                //options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
                  .AddEntityFrameworkStores<FairfieldAllergyDbContext>()
                                    .AddDefaultTokenProviders()
                  .AddTokenProvider<CustomEmailConfirmationTokenProvider
                      <FairfieldIdentityUser>>("CustomEmailConfirmation");



            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToAccessDenied = ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
                options.Events.OnRedirectToLogin = ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);
            });

            //services.ConfigureApplicationCookie(options =>
            //{
            // Cookie settings
            //options.Cookie.HttpOnly = true;
            //options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
            //  options.LoginPath = "/Identity/Account/Login";
            // options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            // options.SlidingExpiration = true;
            //});




            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.StatusCode = 401;
            //
            //                  return Task.CompletedTask;
            //            };
            //      });

            //services.AddControllers();
            services.AddDbContext<FairfieldAllergyDbContext>(opt =>
               opt.UseSqlServer(Configuration.GetConnectionString("FairfieldConnect"))
                  .EnableSensitiveDataLogging()
               );
            services.AddRazorPages();
            Business.Configuration.ConfigurationValues.FairfieldAllergyConnection = "Server = 64.41.86.25; Database = Appointment; Uid = Appointment; Pwd = 0griswold;";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapRazorPages();
            });
        }

        static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(HttpStatusCode statusCode, Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) =>
            context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = (int)statusCode;
                    return Task.CompletedTask;
                }
                return existingRedirector(context);
            };
    }
}
