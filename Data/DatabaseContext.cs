using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpatulaApi.Configurations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SpatulaApi.Data
{
	public class DatabaseContext : IdentityDbContext<ApiUser>
	{
		public DatabaseContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<Course> Courses { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<Lesson> Lessons { get; set; }
		
		public DbSet<UserCourse> UserCourses { get; set; }

		public DbSet<Advert> Adverts { get; set; }

		public DbSet<Review> Reviews { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new RoleConfiguration());
			//var password = new PasswordHasher<ApiUser>();
			//var user = new ApiUser
			//{
			//	Id = "04be4ced-e4cb-408f-8f55-38cd0677cbec",
			//	UserName = "Administrator",
			//	Email = "admin@example.com",
			//	NormalizedEmail = "ADMIN@EXAMPLE.COM",
			//	NormalizedUserName = "ADMINISTRATOR",
			//	FirstName = "Admin"
			//};

			//user.PasswordHash = password.HashPassword(user, "P@ssword123");

			//builder.Entity<ApiUser>().HasData(user);

			//builder.Entity<IdentityUserRole<string>>().HasData(
			//	new IdentityUserRole<string>
			//	{
			//		RoleId = "db99e2c0-7c9e-4c1d-af45-809a2a1f5e7f",
			//		UserId = "04be4ced-e4cb-408f-8f55-38cd0677cbec"
			//	}
			//	);

		}

	}
}
