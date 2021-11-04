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
		public async Task<IActionResult> Create(Lesson createLesson)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (!ModelState.IsValid)
			{
				return NotFound();
			}

			
			createLesson.CreatedDate = DateTime.Now;

			var order = _context.Lessons.Where(l => l.CourseId == createLesson.CourseId).OrderBy(l=>l.Order).LastOrDefault();

			

			createLesson.Order = order!=null?order.Order+1:1;
			createLesson.Status = true;
			_context.Lessons.Add(createLesson);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(int id)
		{

			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}

			var lesson =_context.Lessons.Include(c=>c.Course).FirstOrDefault(c=>c.Id == id);

			if (lesson == null)
			{
				return NotFound();
			}
			ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
			return View(lesson);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Lesson lesson1)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (!ModelState.IsValid)
			{
				return View(lesson1);
			}

			var lesson = _context.Lessons.Find(lesson1.Id);

			lesson.ArabicName = lesson1.ArabicName;
			lesson.Warnings = lesson1.Warnings;
			lesson.Ingredients = lesson1.Ingredients;
			lesson.Utensils = lesson1.Utensils;
			lesson.Description = lesson1.Description;
			lesson.VideoUrl = lesson1.VideoUrl;
			lesson.Status = lesson1.Status;

			_context.Lessons.Update(lesson);

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}

		private bool IsLoggedIn()
		{
			return HttpContext.Session.GetString("UserId") != null;
		}
	}
}
