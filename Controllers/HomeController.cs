using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SpatulaApi.Controllers
{
	public class HomeController : Controller
	{
		#nullable enable
		public IActionResult Index(string? error)
		{
			if(error != null)
			{
				ViewData["error"] = error;
			}
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
	}
}
