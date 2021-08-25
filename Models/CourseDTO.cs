using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SpatulaApi.Models
{


	public class GetCourseDTO
	{
		public int Id { get; set; }

		public string Name { get; set; }
		
		public string Picture { get; set; }
		
		public float? Cost { get; set; }

		public string Description { get; set; }

		public bool Featured { get; set; }

		public bool Status { get; set; }
	}
	public class CreateCourseDTO
	{
		public string Name { get; set; }
		public int CategoryId { get; set; }

		public float? Cost { get; set; }
		public bool Featured { get; set; }
		public string Description { get; set; }

		public IList<IFormFile>? Pictures { get; set; }

	}

	public class UpdateCourseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public float? Cost { get; set; }
		public bool Featured { get; set; }
		public string Description { get; set; }
	}

	public class CourseDTO
	{
		public int Id { get; set; }
		
		public bool Featured { get; set; }

		public bool Status { get; set; }

		public DateTime CreatedDate { get; set; }

		public virtual UserDTO CreatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }
		public string UpdatedById { get; set; }

		public virtual UserDTO UpdatedBy { get; set; } 

		public virtual IList<LessonDTO> Lessons { get; set; }

		public virtual IList<UserCourseDTO> UserCourses { get; set; }

	}
}
