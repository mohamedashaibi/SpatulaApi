using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpatulaApi.Data;

namespace SpatulaApi.Controllers
{
	public class AdvertsController : Controller
	{
		private readonly string LoginError = "يجب ان تسجل الدخول اولا";
		private readonly DatabaseContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;

		public AdvertsController(DatabaseContext context, IWebHostEnvironment hostEnvironment)
		{
			_hostEnvironment = hostEnvironment;
			_context = context;
		}

		// GET: AdvertsController
		public async Task<IActionResult> Index()
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			return View(await _context.Adverts.ToListAsync());
		}

		// GET: AdvertsController/Details/5
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

			var ad = await _context.Adverts
				.FirstOrDefaultAsync(m => m.Id == id);
			if (ad == null)
			{
				return NotFound();
			}

			return View(ad);
		}

		// GET: AdvertsController/Create
		public ActionResult Create()
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			return View();
		}

		// POST: AdvertsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Advert advert, IFormFile image)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (!ModelState.IsValid || image == null)
			{
				return View(advert);
			}

			var order = _context.Adverts.OrderBy(c => c.Order).LastOrDefault().Order;

		
			await _context.Adverts.AddAsync(advert);

			await _context.SaveChangesAsync();

			string coursePhotoPath = Path.Combine(_hostEnvironment.WebRootPath, "images/adverts/");
			if (!Directory.Exists(coursePhotoPath))
			{
				Directory.CreateDirectory(coursePhotoPath);
			}
			var title = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff", CultureInfo.InvariantCulture) + ".jpg";

			using (var stream = new FileStream(Path.Combine(coursePhotoPath, title), FileMode.Create))
			{
				image.CopyTo(stream);
			}

			advert.Image = title;

			_context.Adverts.Update(advert);

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		// GET: AdvertsController/Edit/5
		public ActionResult Edit(int id)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			var ad = _context.Adverts.Find(id);

			if(ad == null)
			{
				return NotFound();
			}


			return View(ad);
		}

		// POST: AdvertsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Advert advert)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			if (!ModelState.IsValid)
			{
				return NotFound();
			}

			_context.Adverts.Update(advert);

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}

		[HttpGet]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			var ad = _context.Adverts.Find(id);

			if (ad == null)
			{
				return NotFound();
			}

			ad.Status = false;

			_context.Adverts.Update(ad);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));

		}

		[HttpGet]
		public async Task<IActionResult> UnDelete(int id)
		{
			if (!IsLoggedIn())
			{
				return RedirectToAction(nameof(Index), "Home", new { error = LoginError });
			}
			var ad = await _context.Adverts.FindAsync(id);
			if (ad == null || ad.Status == true)
			{
				return NotFound();
			}
			ad.Status = true;
			_context.Adverts.Update(ad);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool IsLoggedIn()
		{
			return HttpContext.Session.GetString("UserId") != null;
		}
	}
}
