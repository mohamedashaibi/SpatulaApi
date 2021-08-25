using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpatulaApi.Data;
using SpatulaApi.Models;

namespace SpatulaApi.Controllers
{
	public class LessonsController : Controller
	{
		private readonly string LoginError = "يجب ان تسجل الدخول اولا";
		private readonly DatabaseContext _context;
		private readonly IMapper _mapper;

		public LessonsController(DatabaseContext context, IMapper mapper)
		{
			_mapper = mapper;
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			return View(await _context.Lessons.Include(l=>l.Course).ToListAsync());
		}

		public async Task<IActionResult> CourseLesson(int id)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			var course = await _context.Courses.FindAsync(id);

			if(course == null)
			{
				return NotFound();
			}

			var lessons = await _context.Lessons.Where(l => l.CourseId == course.Id).Include(l=>l.Course).Include(l=>l.CreatedBy).ToListAsync();

			return View(lessons);
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (id == null)
			{
				return NotFound();
			}

			var lesson = await _context.Lessons.Include(l => l.Course).Include(l => l.CreatedBy).FirstOrDefaultAsync(l=>l.Id == id);

			if(lesson == null)
			{
				return NotFound();
			}

			return View(lesson);
		}

		public IActionResult Create()
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
			ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateLessonDTO createLesson)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (!ModelState.IsValid)
			{
				return NotFound();
			}

			var map = _mapper.Map<Lesson>(createLesson);

			map.CreatedDate = DateTime.Now;

			var order = _context.Lessons.Where(l => l.CourseId == createLesson.CourseId).Count() + 1;

			map.Order = order;

			_context.Lessons.Add(map);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		private bool IsLoggedIn()
		{
			return HttpContext.Session.GetString("UserId") != null;
		}
	}
}
