using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

		public AccountsController(UserManager<ApiUser> userManager,
			IMapper mapper, ILogger<AccountsController> logger,
			IAuthManager authManager, IUnitOfWork unitOfWork)
		{
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

			HttpContext.Session.SetString("UserId", user.Id);
			HttpContext.Session.SetString("UserName", user.Email);

			return RedirectToAction("Index", "Courses");
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
