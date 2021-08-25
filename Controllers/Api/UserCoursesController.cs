//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using SpatulaApi.IRepository;
//using SpatulaApi.Models;

//namespace SpatulaApi.Controllers.Api
//{
//	[Authorize()]
//	[Route("api/[controller]")]
//	[ApiController]
//	public class UserCoursesController : ControllerBase
//	{

//		private readonly IUnitOfWork _unitOfWork;
//		private readonly ILogger _logger;
//		private readonly IMapper _mapper;

//		public UserCoursesController(IUnitOfWork unitOfWork, ILogger<UserCoursesController> logger, IMapper mapper)
//		{
//			_unitOfWork = unitOfWork;
//			_logger = logger;
//			_mapper = mapper;
//		}

//		[HttpGet]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//		[ProducesResponseType(StatusCodes.Status400BadRequest)]
//		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
//		public async Task<IActionResult> GetAllUserCourses()
//		{
//			try
//			{
//				var userCourses = await _unitOfWork.UserCourseRepo.GetAll(null, null, new List<string> { "Course", "User" });

//				var userCoursesMap = _mapper.Map<IList<UserCourseDTO>>(userCourses);

//				return Ok(userCoursesMap);

//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, $"There was an error in {nameof(GetAllUserCourses)}");
//				return StatusCode(500, "Internal server error");
//			}
//		}

//	}
//}
