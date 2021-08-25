using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpatulaApi.ExternalContract;

namespace SpatulaApi.Services
{
	public interface IFacebookAuthService
	{
		Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken);

		Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken);

	}
}
