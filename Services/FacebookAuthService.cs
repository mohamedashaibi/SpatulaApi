using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpatulaApi.ExternalContract;

namespace SpatulaApi.Services
{
	public class FacebookAuthService : IFacebookAuthService
	{

		private const string ValidationTokenURL = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
		private const string GetUserInfo = "https://graph.facebook.com/me?fields=id,name,email,picture&access_token={0}";
		private IHttpClientFactory _httpClientFactory;
		private string app_id = "503611874397394";
		private string app_secret = "a704cac1e92bf9c748f6bee2664cda59";
		public FacebookAuthService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken)
		{
			string formattedUrl = string.Format(ValidationTokenURL, accessToken, app_id, app_secret);

			var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
			result.EnsureSuccessStatusCode();


			var resAsString = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<FacebookTokenValidationResult>(resAsString);
		}
		public async Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken)
		{
			string formattedUrl = string.Format(GetUserInfo, accessToken);

			var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
			result.EnsureSuccessStatusCode();

			var resAsString = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<FacebookUserInfoResult>(resAsString);
		}

		
	}
}
