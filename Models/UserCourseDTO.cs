using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Models
{

	public class CreateUserCourseDTO
	{
		public int CourseId { get; set; }

		public string UserId { get; set; }

	}

	public class UserCourseDTO : CreateUserCourseDTO
	{
		public int Id { get; set; }

		public virtual UserDTO User { get; set; }

		public virtual CreateCourseDTO Course { get; set; }

		public DateTime AddedDate { get; set; }

		public int LessonReachedId { get; set; }

		public virtual LessonDTO LessonReached { get; set; }
	}
}
