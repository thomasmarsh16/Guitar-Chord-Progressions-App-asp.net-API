using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GuitarChordProgressions.services;
using System.Data.SqlClient;

namespace GuitarChordProgressions
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
            services.AddCors(options =>
            {
                options.AddPolicy("GuitarAngularApp",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:4200", "http://localhost:4200");
                    });
            });
            services.AddSingleton<IProgressionRepository, ProgressionRepository>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static async Task<ProgressionRepository> InitializeProgressionDBAsync(IConfigurationSection configurationSection)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = configurationSection.GetSection("DataSource").Value;
            builder.UserID = configurationSection.GetSection("UID").Value;
            builder.Password = configurationSection.GetSection("Pass").Value;
            builder.InitialCatalog = configurationSection.GetSection("Catalog").Value;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                ProgressionRepository progressionService = new ProgressionRepository(connection);

                return progressionService;
            }
        }

    }
}
