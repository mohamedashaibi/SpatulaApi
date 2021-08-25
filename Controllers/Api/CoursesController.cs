using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpatulaApi.Data;
using SpatulaApi.IRepository;
using SpatulaApi.Models;

namespace SpatulaApi.Controllers.Api
{
		[Authorize()]
		[Route("api/[controller]")]
		[ApiController]
	public class CoursesController : ControllerBase
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _hostingEnvironment;

		public CoursesController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, ILogger<CoursesController> logger, IMapper mapper)
		{
			_hostingEnvironment = webHostEnvironment;
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllCourses()
		{
			try
			{
				var courses = await _unitOfWork.CourseRepo.GetAll(c=>c.Status == true, c=>c.OrderBy(c=>c.CreatedDate));
				var coursesMap = _mapper.Map<IList<GetCourseDTO>>(courses);
				return Ok(coursesMap);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"There wass an error in {nameof(GetAllCourses)}");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("usercourses")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetUserCourses()
		{
			if (GetUserId() == null)
			{
				return BadRequest("You are not logged in");
			}
			try
			{
				var user = await _unitOfWork.UserRepo.Get(u => u.Id == GetUserId());
				if (user == null)
				{
					return BadRequest("You are not logged in");
				}

				var courses = await _unitOfWork.UserCourseRepo.GetAll(c => c.UserId == user.Id, null, new List<string> { "Course" });

				return Ok(courses);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occured in {nameof(GetUserCourses)}");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("coursesWithCategory/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetCoursesWithCategory(int id)
		{

			try
			{

				var category = await _unitOfWork.CategoryRepo.Get(c => c.Id == id);
				if(category == null)
				{
					return BadRequest("Category not found");
				}

				var user = await _unitOfWork.UserRepo.Get(u => u.Id == GetUserId());
				if (user == null)
				{
					return BadRequest("You are not logged in");
				}

				var courses = await _unitOfWork.UserCourseRepo.GetAll(c => c.UserId == user.Id, null, new List<string> { "Course" });

				return Ok(courses);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occured in {nameof(GetUserCourses)}");
				return StatusCode(500, "Internal server error");
			}
		}


		[HttpGet("{id:int}", Name = "GetCourse")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetCourse(int id)
		{
			if (id < 1)
			{
				return BadRequest("Invalid data");
			}
			try
			{
				var course = await _unitOfWork.CourseRepo.Get(c => c.Id == id && c.Status == true);

				if (course == null)
				{
					return BadRequest("No course found with this id");
				}


				var hasCourse = await _unitOfWork.UserCourseRepo.GetWithMulti(new List<Expression<Func<UserCourse, bool>>> { u=>u.UserId == GetUserId(), u=>u.CourseId == course.Id});

				if(hasCourse == null)
				{
					return BadRequest("You do not own this course");
				}

				var courseMap = _mapper.Map<GetCourseDTO>(course);


				return Ok(courseMap);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"There wass an error in {nameof(GetCourse)}");
				return StatusCode(500, "Internal server error");
			}
		}

		//[HttpPost]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO courseDTO)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		_logger.LogError("Error in model when creating course");
		//		return BadRequest("Invalid data");
		//	}


		//	try
		//	{
		//		var course = _mapper.Map<Course>(courseDTO);
		//		course.Status = true;
		//		await _unitOfWork.CourseRepo.Create(course);
		//		await _unitOfWork.Save();

		//		var courseMap = _mapper.Map<GetCourseDTO>(course);

		//		if (courseDTO.Pictures != null)
		//		{
		//			string coursePhotoPath = Path.Combine(_hostingEnvironment.WebRootPath, "images/" + course.Id);
		//			if (!Directory.Exists(coursePhotoPath))
		//			{
		//				Directory.CreateDirectory(coursePhotoPath);
		//			}
		//			foreach (var image in courseDTO.Pictures)
		//			{

		//				var title = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff", CultureInfo.InvariantCulture) + ".jpg";

		//				using (var stream = new FileStream(Path.Combine(coursePhotoPath, title), FileMode.Create))
		//				{
		//					image.CopyTo(stream);
		//					course.Picture += "," + title;
		//				}


		//			}

		//			_unitOfWork.CourseRepo.Update(course);
		//			await _unitOfWork.Save();

		//		}

		//		return CreatedAtRoute("GetCourse", new { id = course.Id }, courseMap);

		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"There was an error in {nameof(GetCourse)}");
		//		return StatusCode(500, "Internal server error");
		//	}

		//}

		//[HttpPut("{id:int}")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> UpdateCourse(int id, [FromBody] GetCourseDTO courseDTO)
		//{
		//	if (!ModelState.IsValid || id < 1)
		//	{
		//		return BadRequest("Invalid data");
		//	}

		//	try
		//	{
		//		var course = await _unitOfWork.CourseRepo.Get(c => c.Id == id);

		//		if (course == null)
		//		{
		//			return BadRequest("No course found with this id");
		//		}

		//		var courseMap = _mapper.Map(courseDTO, course);

		//		_unitOfWork.CourseRepo.Update(courseMap);

		//		await _unitOfWork.Save();

		//		return NoContent();

		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"There was an error in {nameof(GetCourse)}");
		//		return StatusCode(500, "Internal server error");
		//	}

		//}

		//[HttpDelete("{id:int}")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> DeleteCourse(int id)
		//{
		//		try
		//		{
		//			var course = await _unitOfWork.CourseRepo.Get(c => c.Id == id);

		//			if (course == null)
		//			{
		//				return BadRequest("No course found with this id");
		//			}

		//			course.Status = false;

		//			_unitOfWork.CourseRepo.Update(course);

		//			await _unitOfWork.Save();

		//			return NoContent();

		//		}
		//		catch (Exception ex)
		//		{
		//			_logger.LogError(ex, $"There was an error in {nameof(GetCourse)}");
		//			return StatusCode(500, "Internal server error");
		//		}
		//}

		private string GetUserId()
		{
			ClaimsPrincipal principal = User;

			return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value;
		}

	}
}
