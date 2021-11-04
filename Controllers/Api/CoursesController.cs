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


		[HttpGet("addFreeCourse/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddFreeCourse(int id)
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

				var course = await _unitOfWork.CourseRepo.Get(c => c.Id == id);

				if(course.Cost != 0) {
					return BadRequest("This course is paid");
				}

				var courseExists = _unitOfWork.UserCourseRepo.GetWithMulti(new List<Expression<Func<UserCourse, bool>>> { c => c.UserId == GetUserId(), c => c.CourseId == id });
				
				if(courseExists != null)
				{
					return BadRequest("Course is already registered for this user");
				}

				var userCourse = new UserCourse
				{
					CourseId = id,
					UserId = GetUserId(),
					AddedDate = DateTime.Now
				};

				await _unitOfWork.UserCourseRepo.Create(userCourse);

				await _unitOfWork.Save();

				return Ok("Course Added");

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

				var courses = await _unitOfWork.CourseRepo.GetAll(u => u.CategoryId == category.Id);
			
				var coursesMap = _mapper.Map<IList<CourseDTO>>(courses);

				return Ok(coursesMap);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occured in {nameof(GetUserCourses)}");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("freecourses")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetFreeCourses()
		{

			try
			{

				var courses = await _unitOfWork.CourseRepo.GetWithMulti(new List<Expression<Func<Course, bool>>> { c => c.Category.EnglishName == "Free", c => c.Status == true });

				var coursesMap = _mapper.Map<IList<GetCourseDTO>>(courses);

				return Ok(coursesMap);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occured in {nameof(GetUserCourses)}");
				return StatusCode(500, "Internal server error");
			}
		}



		[HttpGet("paidcourses")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetPaidCourses()
		{

			try
			{

				var courses = await _unitOfWork.CourseRepo.GetWithMulti(new List<Expression<Func<Course, bool>>> { c => c.Category.EnglishName == "Paid", c => c.Status == true });

				var coursesMap = _mapper.Map<IList<GetCourseDTO>>(courses);

				return Ok(coursesMap);

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


				//var hasCourse = await _unitOfWork.UserCourseRepo.GetWithMulti(new List<Expression<Func<UserCourse, bool>>> { u=>u.UserId == GetUserId(), u=>u.CourseId == course.Id});

				//if(hasCourse == null)
				//{
				//	return BadRequest("You do not own this course");
				//}

				var courseMap = _mapper.Map<GetCourseDTO>(course);


				return Ok(courseMap);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"There wass an error in {nameof(GetCourse)}");
				return StatusCode(500, "Internal server error");
			}
		}


		private string GetUserId()
		{
			ClaimsPrincipal principal = User;

			return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value;
		}

	}
}
