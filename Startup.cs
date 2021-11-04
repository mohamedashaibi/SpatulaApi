using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpatulaApi.Data;
using SpatulaApi.IRepository;
using SpatulaApi.Repository;
using SpatulaApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SpatulaApi.Configurations;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Identity;

namespace SpatulaApi
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

			services.AddDbContext<DatabaseContext>(options=>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DbConnection")).EnableDetailedErrors();
				//sqlServerOptionsAction: option =>
				//{
				//	option.EnableRetryOnFailure();
				//});
				
			});

			services.AddAuthentication();

			services.ConfigureIdentityServices();

			services.AddDefaultIdentity<ApiUser>()
				.AddRoles<IdentityRole>()
				.AddDefaultUI()
				.AddEntityFrameworkStores<DatabaseContext>();

			services.ConfigureJWT(Configuration);

			services.AddCors(o=>
			{
				o.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyOrigin();
					policy.AllowAnyHeader();
					policy.AllowAnyMethod();
				});
			});

			services.AddSession();

			services.ConfigureRateLimiting();

			services.AddHttpContextAccessor();

			services.AddMemoryCache();

			services.ConfigureVersioning();

			services.AddTransient<IFacebookAuthService, FacebookAuthService>();

			services.AddTransient<IUnitOfWork, UnitOfWork>();

			services.AddAutoMapper(typeof(MapperInitializer));

			services.AddScoped<IAuthManager, AuthManager>();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpatulaApi", Version = "v1" });
			});

			services.AddControllersWithViews();

			services.AddControllers().AddNewtonsoftJson(op =>
			{
				op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			});

			services.AddHttpClient();

			services.AddAuthorization();
			
			services.AddAuthentication().AddFacebook(facebookOptions =>
			{
				facebookOptions.AppId = "503611874397394";
				facebookOptions.AppSecret = "a704cac1e92bf9c748f6bee2664cda59";
			});

			services.AddMvc(options =>
			  {
				  options.EnableEndpointRouting = false;
			  });
			
			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpatulaApi v1"));

			app.UseSession();

			app.UseHttpsRedirection();

			app.UseCors("AllowAll");

			app.UseRouting();

			app.UseAuthentication();

			app.UseStaticFiles();

			app.UseAuthorization();

			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapControllerRoute(
			//		name: "default",
			//		pattern: "{controller=Home}/{action=Index}/{id?}");
			//});

			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapControllers();
			//});


			//app.UseMvcWithDefaultRoute();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseMvcWithDefaultRoute();

			app.UseMvc();

			app.UseIpRateLimiting();

		}
	}
}
