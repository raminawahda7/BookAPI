using BookAPI.Data;
using BookAPI.Repositories;
using Domain;
using Domain.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BookAPI
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

            services.AddDbContextPool<AppDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("BookDBConnection"), b => b.MigrationsAssembly("BookAPI").UseNetTopologySuite()));
            services.AddControllers();
            services.AddSwaggerGen(c =>

            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthorPublisherAPI", Version = "v1" });
            });
            //services.AddMvc().AddXmlSerializerFormatters();
            services.AddScoped<IAuthorManager, AuthorManager>();
            services.AddScoped<IPublisherManager, PublisherManager>();

            services.AddScoped<IRepository<Author, int>, SQLAuthorRepository>();
            services.AddScoped<IRepository<Publisher, int>, SQLPublisherRepository>();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookAPI"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x =>
                x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
