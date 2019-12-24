using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cashflowio.Core.SharedKernel;
using Cashflowio.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Syncfusion.Licensing;

namespace Cashflowio.Web
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddControllersAsServices()
                .AddRazorRuntimeCompilation()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "My API", Version = "v1"}); });

            return BuildDependencyInjectionProvider(services);
        }

        private static IServiceProvider BuildDependencyInjectionProvider(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            var webAssembly = Assembly.GetExecutingAssembly();
            var coreAssembly = Assembly.GetAssembly(typeof(BaseEntity));
            var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));
            builder.RegisterAssemblyTypes(webAssembly, coreAssembly, infrastructureAssembly).AsImplementedInterfaces();

            var applicationContainer = builder.Build();
            return new AutofacServiceProvider(applicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            SyncfusionLicenseProvider.RegisterLicense(
                "MTUyNzI4QDMxMzcyZTMzMmUzMGJVMW1sek0xcTJDTVV5NEtwUml4b3hwd1E5ZmE5VDVTWk9ydURaRUlUdjA9;MTUyNzI5QDMxMzcyZTMzMmUzMGN6NWE0S0tDNDV2MU0zTmdzbEVwdzZTOWFXS0NSN0NERnZPQjN5bERjSGM9;MTUyNzMwQDMxMzcyZTMzMmUzMGJVMW1sek0xcTJDTVV5NEtwUml4b3hwd1E5ZmE5VDVTWk9ydURaRUlUdjA9");

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                //app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}