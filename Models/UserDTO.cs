using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Models
{

	public class AuthUser
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }

	}

	public class UserLoginDTO
	{

		[Required(ErrorMessage = "هذا الحقل اجباري")]
		[DisplayName("البريد الالكتروني")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "هذا الحقل اجباري")]
		[DisplayName("كلمة المرور")]
		[DataType(DataType.Password)]
		[StringLength(15, ErrorMessage = "هذا الحقل يجب ان يتكون من 10 الى 15 خانة", MinimumLength = 10)]
		public string Password { get; set; }
	}
	public class UserDTO : UserLoginDTO
	{

		public string FirstName { get; set; }
		public string LastName { get; set; }
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		public ICollection<string> Roles { get; set; }
 
	}
}
