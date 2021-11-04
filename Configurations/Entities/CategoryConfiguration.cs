using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpatulaApi.Data;

namespace SpatulaApi.Configurations.Entities
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasData(
				new Category
				{
					Id = 1,
					ArabicName = "مجاني",
					EnglishName = "Free",
					Status = true
				}, 
				new Category
				{
					Id = 2,
					ArabicName = "مدفوع",
					EnglishName = "Paid",
					Status = true
				}
				);
		}
	}
}
