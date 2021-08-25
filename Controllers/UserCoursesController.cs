using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpatulaApi.Data;

namespace SpatulaApi.Controllers
{
	public class UserCoursesController : Controller
	{
		private readonly string LoginError = "يجب ان تسجل الدخول اولا";

		private readonly DatabaseContext _context;
		public UserCoursesController(DatabaseContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			return View(await _context.UserCourses.Include(u=>u.Course).Include(u=>u.User).Include(l=>l.LessonReached).ToListAsync());
		}

		public IActionResult Create()
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserCourse userCourse)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (!ModelState.IsValid)
			{
				return NotFound();
			}

			userCourse.AddedDate = DateTime.Now;

			_context.UserCourses.Add(userCourse);

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
		private bool IsLoggedIn()
		{
			return HttpContext.Session.GetString("UserId") != null;
		}
	}
}