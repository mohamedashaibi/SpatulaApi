using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpatulaApi.Data;
using SpatulaApi.IRepository;
using SpatulaApi.Models;
using SpatulaApi.Services;

namespace SpatulaApi.Controllers
{
	public class AccountsController : Controller
	{
		private readonly UserManager<ApiUser> _userManager;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IAuthManager _authManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly DatabaseContext _context;

		public AccountsController(UserManager<ApiUser> userManager,
			IMapper mapper, ILogger<AccountsController> logger,
			IAuthManager authManager, IUnitOfWork unitOfWork, 
			DatabaseContext context)
		{
			_context = context;
			_userManager = userManager;
			_mapper = mapper;
			_logger = logger;
			_authManager = authManager;
			_unitOfWork = unitOfWork;
		}


		public IActionResult Details(string id)
		{
			if(id == "")
			{
				return NotFound();
			}

			var user = _unitOfWork.UserRepo.Get(u => u.Id == id);

			if(user == null)
			{
				return NotFound();
			}

			return View(user);
		}
		

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
		{

			if (!ModelState.IsValid)
			{
				return View(userLoginDTO);
			}

			if (!await _authManager.ValidateUser(userLoginDTO))
			{
				return Unauthorized();
			}

			var user = await _unitOfWork.UserRepo.Get(u => u.Email == userLoginDTO.Email);

			var mappedUser = _mapper.Map<AuthUser>(user);


			if (await _userManager.IsInRoleAsync(user, "Administrator"))
			{
				HttpContext.Session.SetString("UserId", user.Id);
				HttpContext.Session.SetString("UserName", user.Email);
				return RedirectToAction("Index", "Courses");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("This user is not an admin");
				return View(userLoginDTO);
			}
		}


		[Authorize(Roles = "User")]
		public async  Task<IActionResult> DeleteAccount()
		{
			try
			{
				ClaimsPrincipal principal = User;

				var id = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value;

				var user = await _userManager.FindByIdAsync(id);

				var logins = await _userManager.GetLoginsAsync(user);

				var rolesForUser = await _userManager.GetRolesAsync(user);

				using (var transaction = _context.Database.BeginTransaction())
				{
					foreach (var login in logins.ToList())
					{
						await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
					}

					if (rolesForUser.Count > 0)
					{
						foreach (var item in rolesForUser.ToList())
						{
							// item should be the name of the role
							var result = await _userManager.RemoveFromRoleAsync(user, item);
						}
					}

					await _userManager.DeleteAsync(user);
					transaction.Commit();
					return Ok("Deleted successfully");
				}
			}catch(Exception ex)
			{
				return BadRequest("Error in deleting user" + ex.Message);
			}

		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
