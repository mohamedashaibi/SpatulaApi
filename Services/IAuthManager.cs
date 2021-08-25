using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpatulaApi.Data;
using SpatulaApi.Models;

namespace SpatulaApi.Services
{
	public interface IAuthManager
	{
		Task<bool> ValidateUser(UserLoginDTO userDTO);
		Task<bool> ValidateFacebookUser(UserLoginDTO userDTO);
		Task<string> CreateToken();
	}
}
