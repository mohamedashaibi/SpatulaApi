using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Models
{
	public class LessonGetDTO
	{
		public int Id { get; set; }

		public string Warnings { get; set; }

		public string ArabicName { get; set; }

		public string Description { get; set; }

		public string Utensils { get; set; }

		public string Ingredients { get; set; }

		public string VideoUrl { get; set; }

		public int CourseId { get; set; }

		public int Order { get; set; }

		public bool Status { get; set; }

	}

	public class LessonListDTO
	{
		public int Id { get; set; }

		public string Warnings { get; set; }

		public string ArabicName { get; set; }

		public string Description { get; set; }

		public int CourseId { get; set; }

		public int Order { get; set; }

		public bool Status { get; set; }

	}
	public class CreateLessonDTO
	{
		public int CourseId { get; set; }

		public string Warnings { get; set; }

		public string ArabicName { get; set; }

		public string Description { get; set; }

		public string Utensils { get; set; }

		public string Ingredients { get; set; }

		public string VideoUrl { get; set; }

		public int Order { get; set; }
	}
	public class LessonDTO : CreateLessonDTO
	{
		public int Id { get; set; }
		public bool Status { get; set; }

		public DateTime CreatedDate { get; set; }

		public string CreatedById { get; set; }

		public virtual UserDTO CreatedBy { get; set; }

		public virtual CourseDTO Course { get; set; }

	}
}
