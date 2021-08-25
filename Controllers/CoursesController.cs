using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpatulaApi.Data;
using SpatulaApi.IRepository;
using SpatulaApi.Models;

namespace SpatulaApi.Controllers
{
	public class CoursesController : Controller
	{
		private readonly string LoginError = "يجب ان تسجل الدخول اولا";
		private readonly DatabaseContext _context;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;

		public CoursesController(DatabaseContext context, IMapper mapper, IUnitOfWork unitOfWork
			, IWebHostEnvironment hostEnvironment)
		{
			_hostEnvironment = hostEnvironment;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError});
			}
			return View(await _context.Courses.ToListAsync());
		}
		public IActionResult Create()
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "ArabicName");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateCourseDTO courseDTO )
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (!ModelState.IsValid)
			{
				return NotFound();
			}

			var map = _mapper.Map<Course>(courseDTO);

			map.CreatedDate = DateTime.Now;
			map.Status = true;
			await _unitOfWork.CourseRepo.Create(map);
			await _unitOfWork.Save();

			if(courseDTO.Pictures != null)
			{
				string coursePhotoPath = Path.Combine(_hostEnvironment.WebRootPath, "images/courses/" + map.Id);
				if (!Directory.Exists(coursePhotoPath))
				{
					Directory.CreateDirectory(coursePhotoPath);
				}
				foreach (var image in courseDTO.Pictures)
				{

					var title = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff", CultureInfo.InvariantCulture) + ".jpg";

					using (var stream = new FileStream(Path.Combine(coursePhotoPath, title), FileMode.Create))
					{
						image.CopyTo(stream);
						map.Picture += "," + title;
					}


				}
				var newPic = map.Picture;
				if (newPic.ToCharArray().Last() == ',')
				{
					newPic = newPic.Remove(newPic.Length - 1, 1);
				}
				if (newPic.ToCharArray().First() == ',')
				{
					newPic = newPic.Remove(0, 1);
				}

				map.Picture = newPic;

				_unitOfWork.CourseRepo.Update(map);
				await _unitOfWork.Save();

			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> AddLesson(int? id)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (id == null)
			{
				return NotFound();
			}

			var course = await _context.Courses.FindAsync(id);

			if(course == null)
			{
				return NotFound();
			}
			ViewData["CourseId"] = course.Id.ToString();
			return View();
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

			var course = await _context.Courses.Include(c=>c.Lessons).Include(c=>c.Category).FirstOrDefaultAsync(c=>c.Id == id);
			if(course == null)
			{
				return NotFound();
			}

			return View(course);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (id == null)
			{
				return NotFound();
			}

			var course = await _context.Courses.FindAsync(id);

			if(course == null)
			{
				return NotFound();
			}
			var map = _mapper.Map<UpdateCourseDTO>(course);
			ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "ArabicName");
			return View(map);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateCourseDTO courseDTO)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (!ModelState.IsValid)
			{
				return View(courseDTO);
			}

			var course = _context.Courses.Find(courseDTO.Id);

			course.Name = courseDTO.Name;
			course.Cost = courseDTO.Cost;
			course.CategoryId = courseDTO.CategoryId;
			course.Description = courseDTO.Description;
			course.Featured = courseDTO.Featured;

			_context.Courses.Update(course);

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> DeleteImage(string id, int courseid)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (id == null)
			{
				return NotFound();
			}

			var courseimage = await _context.Courses.Where(c=>c.Id == courseid).Where(c => c.Picture.Contains(id)).FirstOrDefaultAsync();

			if(courseimage == null)
			{
				return NotFound();
			}
			var newPic = "";
			foreach(var pic in courseimage.Picture.Split(","))
			{
				if (pic != id)
				{
					newPic += pic + ",";
				}
			}
			newPic = newPic.Replace(",,", ",");

			if(newPic.ToCharArray().Last() == ',')
			{
				newPic = newPic.Remove(newPic.Length - 1, 1);
			}
			if (newPic.ToCharArray().First() == ',')
			{
				newPic = newPic.Remove(0, 1);
			}
			courseimage.Picture = newPic;

			string coursePhotoPath = Path.Combine(_hostEnvironment.WebRootPath, "images/courses/" + courseimage.Id);

			FileInfo file = new FileInfo(coursePhotoPath + "/" + id);
			if (file.Exists)
			{
				file.Delete();
			}
			_context.Courses.Update(courseimage);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Details), new { id = courseimage.Id });
		}

		private bool IsLoggedIn()
		{
			return HttpContext.Session.GetString("UserId") != null;
		}
	}
}
