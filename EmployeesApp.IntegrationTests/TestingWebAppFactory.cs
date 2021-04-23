using EmployeesApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EmployeesApp.IntegrationTests
{
	public class TestingWebAppFactory : WebApplicationFactory<Startup>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				var descriptor = services.SingleOrDefault(
					d => d.ServiceType == typeof(DbContextOptions<EmployeeContext>));
				if (descriptor != null)
				{
					services.Remove(descriptor);
				}

				var serviceProvider = new ServiceCollection()
					.AddEntityFrameworkInMemoryDatabase()
					.BuildServiceProvider();
				services.AddDbContext<EmployeeContext>(options =>
				{
					options.UseInMemoryDatabase("InMemoryEmployeeTest");
					options.UseInternalServiceProvider(serviceProvider);
				});
				services.AddAntiforgery(options => 
				{
					options.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
					options.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
				});

				var sp = services.BuildServiceProvider();
				using var scope = sp.CreateScope();
				using var appContext = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
				try
				{
					appContext.Database.EnsureCreated();
				}
				catch (Exception)
				{
					//Log errors or do anything you think it's needed
					throw;
				}
			});
		}
	}
}
