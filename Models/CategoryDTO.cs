using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Models
{
	public class CreateCategoryDTO
	{
		public string ArabicName { get; set; }
		public string EnglishName { get; set; }
	}
	public class CategoryDTO : CreateCategoryDTO
	{
		public int Id { get; set; }

		public bool Status { get; set; }

		public virtual IList<CourseDTO> Courses { get; set; }
	}
}
