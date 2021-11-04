using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SpatulaApi.Data;
using SpatulaApi.Models;
using SpatulaApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpatulaApi.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SpatulaApi.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{

		private readonly UserManager<ApiUser> _userManager;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IAuthManager _authManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFacebookAuthService _facebookAuth;

		public AccountController(UserManager<ApiUser> userManager,
			IMapper mapper, ILogger<AccountController> logger, IFacebookAuthService facebookAuthService,
			IAuthManager authManager, IUnitOfWork unitOfWork)
		{
			_facebookAuth = facebookAuthService;
			_userManager = userManager;
			_mapper = mapper;
			_logger = logger;
			_authManager = authManager;
			_unitOfWork = unitOfWork;
		}

		//[HttpPost]
		//[Route("register")]
		//public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}
		//	try
		//	{
		//		var user = _mapper.Map<ApiUser>(userDTO);
		//		user.UserName = userDTO.Email;
		//		var result = await _userManager.CreateAsync(user, userDTO.Password);
		//		if (!result.Succeeded)
		//		{
		//			_logger.LogError("Something went wrong in the registration.");
		//			string errors = "";
		//			foreach(var item in result.Errors)
		//			{
		//				errors += item.Description + "//" + item.Code + "\n";
		//			}
		//			_logger.LogError(errors);

		//			return BadRequest("Something went wrong when registering.");
		//		}
		//		await _userManager.AddToRolesAsync(user, userDTO.Roles);
		//		return Ok(user);
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"Something went wrong in {nameof(Register)}");
		//		return Problem("Something went wrong", statusCode: 500);
		//	}

		//}

		//[HttpPost]
		//[Route("login")]
		//public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}
		//	try
		//	{

		//		if(!await _authManager.ValidateUser(userLoginDTO))
		//		{
		//			return Unauthorized();
		//		}

		//		var user = await _unitOfWork.UserRepo.Get(u => u.Email == userLoginDTO.Email);

		//		var mappedUser = _mapper.Map<AuthUser>(user);

		//		return Accepted(new { Token = _authManager.CreateToken().Result, User =  mappedUser});
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"Something went wrong in {nameof(Register)}");
		//		return Problem("Something went wrong", statusCode: 500);
		//	}

		//}

		[HttpGet]
		[Route("facebooklogin")]
		public async Task<IActionResult> LoginFacebook(string accessToken)
		{

			System.Diagnostics.Debug.WriteLine("We are in facebooklogin api call");
			var validation = await _facebookAuth.ValidateAccessTokenAsync(accessToken);
			if (!validation.Data.IsValid)
			{
				_logger.LogError("Error logging into facebook with accesstoken " + accessToken);
				return BadRequest("Invalid login");
			}

			var userInfo = await _facebookAuth.GetUserInfoAsync(accessToken);

			if(userInfo == null)
			{
				return Unauthorized("Error");
			}

			if(userInfo.Email == null)
			{
				var user1 = await _userManager.FindByEmailAsync(userInfo.Name.Split(" ")[0] + userInfo.Name.Split(" ")[1] + "@yes.com");

				if(user1 == null)
				{
					return BadRequest("User is not registered");
				}

				var usermapped1 = _mapper.Map<UserLoginDTO>(user1);

				if (!await _authManager.ValidateFacebookUser(usermapped1))
				{
					return Unauthorized();
				}

				System.Diagnostics.Debug.WriteLine("mapping user ok");

				return Accepted(new { Token = _authManager.CreateToken().Result, User = user1, FacebookUser = userInfo });
			}

			var user = await _userManager.FindByEmailAsync(userInfo.Email);

			if (user == null)
			{
				System.Diagnostics.Debug.WriteLine("user is not registered");
				return BadRequest("This user is not registered, please register first");
			}

			var usermapped = _mapper.Map<UserLoginDTO>(user);

			if (!await _authManager.ValidateFacebookUser(usermapped))
			{
				return Unauthorized();
			}

			System.Diagnostics.Debug.WriteLine("mapping user");

			return Accepted(new { Token = _authManager.CreateToken().Result, User = user, FacebookUser = userInfo });
		}

		[HttpPost]
		[Route("facebookregister")]
		public async Task<IActionResult> RegisterFacebook(string accessToken)
		{
			var validation = await _facebookAuth.ValidateAccessTokenAsync(accessToken);
			if (!validation.Data.IsValid)
			{
				_logger.LogError("Error logging into facebook with accesstoken " + accessToken);
				return BadRequest("Invalid login");
			}

			var userInfo = await _facebookAuth.GetUserInfoAsync(accessToken);

			System.Diagnostics.Debug.WriteLine("Email=" + userInfo.Email + " Id" + userInfo.Id);

			if (userInfo == null)
			{
				return Unauthorized("Error");
			}

			if(userInfo.Email == null)
			{
				//Login with phonenumber
				var userPhone = await _userManager.FindByLoginAsync("Faceboook", accessToken);

				if (userPhone != null)
				{
					return BadRequest("User already exists");
				}
				var userRegister1 = new ApiUser
				{
					Id = Guid.NewGuid().ToString(),
					UserName = userInfo.Name.Split(" ")[0] + userInfo.Name.Split(" ")[1] + Guid.NewGuid().ToString(), 
					FirstName = userInfo.Name.Split(" ")[0],
					LastName = userInfo.Name.Split(" ")[1],
					Email = userInfo.Name.Split(" ")[0]+ userInfo.Name.Split(" ")[1]+"@yes.com"
				};
				var userCreated1 = await _userManager.CreateAsync(userRegister1);


				if (!userCreated1.Succeeded)
				{
					_logger.LogError("Something went wrong in the registration.");
					string errors = "";
					foreach (var item in userCreated1.Errors)
					{
						errors += item.Description + "//" + item.Code + "\n";
					}
					_logger.LogError(errors);

					return BadRequest("Something went wrong when registering.");
				}


				await _userManager.AddToRoleAsync(userRegister1, "User");

				var usermap1 = _mapper.Map<UserLoginDTO>(userRegister1);

				if (!await _authManager.ValidateFacebookUser(usermap1))
				{
					return BadRequest();
				}

				return Accepted(new { Token = _authManager.CreateToken().Result, User = userRegister1, FacebookUser = userInfo });
			}

			var user = await _userManager.FindByEmailAsync(userInfo.Email);

			if (user != null)
			{
				System.Diagnostics.Debug.WriteLine("user is not registered");
				return BadRequest("This user registered, please proceed to the login page, if you have forgotten your password pleas visit the forgot password link or contact the support team for further help.");
			}

			var userRegister = new ApiUser
			{
				Id = Guid.NewGuid().ToString(),
				FirstName = userInfo.Name.Split(" ")[0],
				LastName = userInfo.Name.Split(" ")[1],
				UserName = userInfo.Email,
				Email = userInfo.Email
			};

			var userCreated = await _userManager.CreateAsync(userRegister);


			if (!userCreated.Succeeded)
			{
				_logger.LogError("Something went wrong in the registration.");
				string errors = "";
				foreach (var item in userCreated.Errors)
				{
					errors += item.Description + "//" + item.Code + "\n";
				}
				_logger.LogError(errors);

				return BadRequest("Something went wrong when registering.");
			}


			await _userManager.AddToRoleAsync(userRegister, "User");

			var usermap = _mapper.Map<UserLoginDTO>(userRegister);

			if(!await _authManager.ValidateFacebookUser(usermap))
			{
				return BadRequest();
			}

			return Accepted(new { Token = _authManager.CreateToken().Result, User = userRegister, FacebookUser = userInfo });
		}

		[Authorize()]
		[HttpGet("{id}", Name = "GetProfile")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetProfile(string id)
		{
			if (GetUserId() == null)
			{
				return BadRequest("Error");
			}

			var user = await _unitOfWork.UserRepo.Get(u => u.Id == id);

			if(user == null)
			{
				return BadRequest("User not found");
			}

			var mappedUser = _mapper.Map<UserDTO>(user);

			return Ok(mappedUser);
		}
		private string GetUserId()
		{
			ClaimsPrincipal principal = User;

			return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value;
		}

	}
}
