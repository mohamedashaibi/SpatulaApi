using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SpatulaApi.Data
{
	public class ApiUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public virtual IList<UserCourse> UserCourses { get; set; }

	}
}
