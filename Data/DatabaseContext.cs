using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpatulaApi.Configurations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new RoleConfiguration());

		}

	}
}
