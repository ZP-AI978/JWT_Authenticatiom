using Buyer.Core.User;
using Buyer.Core.User.Support;
using Context.Context;
using Domain.Entitities.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Security.Cryptography;
using Token.Jwt;

namespace API
{
    public class Startup
    {
        private JwtSettings jwtSettings;
        private string connectionString;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IJwtGenerator, JwtGenerator>();
            services.AddTransient<IJwtValidator, JwtValidator>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUserLookup, UserLookup>();
            services.AddTransient<IUserRoleLookup, UserRoleLookup>();

            AddSettings(services);
            GetConfig();

            services.AddDbContext<BuyertContext>(options => options.UseSqlServer(connectionString));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.TokenValidationParameters = GetTokenValidationParameters();
            });


            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Buyer API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
            });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            //});
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
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void AddSettings(IServiceCollection services)
        {
            services.AddSingleton(Configuration.GetSection("JwtSettings").Get<JwtSettings>());
            services.AddSingleton(Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>());
        }
        private void GetConfig()
        {
            jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            connectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            string strPublicKey = jwtSettings.PublicKeyXml;
            RSA publicRsa = RSA.Create();
            publicRsa.FromXmlString(strPublicKey);

            var publicKey = new RsaSecurityKey(publicRsa);
            RsaSecurityKey signingKey = new RsaSecurityKey(publicRsa);

            var myIssuer = jwtSettings.Issuer;
            var myAudience = jwtSettings.Audience;

            return new TokenValidationParameters()
            {
                IssuerSigningKey = signingKey,
                ValidateAudience = true,
                ValidAudience = myAudience,
                ClockSkew = TimeSpan.FromMinutes(5),
                ValidateIssuer = true,
                ValidIssuer = myIssuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        }
    }
}
