using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SpatulaApi.Configurations.Entities
{
	public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			builder.HasData(
				new IdentityRole
				{
					Id = "a6d7ae48-c34c-4224-80ef-4dc5e53267ce",
					Name = "User",
					NormalizedName = "USER"
				},
				new IdentityRole
				{
					Id = "db99e2c0-7c9e-4c1d-af45-809a2a1f5e7f",
					Name= "Administrator",
					NormalizedName="ADMINISTRATOR"
				}
				);
		}
	}
}
