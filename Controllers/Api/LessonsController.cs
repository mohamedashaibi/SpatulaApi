using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpatulaApi.Data;
using SpatulaApi.IRepository;
using SpatulaApi.Models;

namespace SpatulaApi.Controllers.Api
{
	[Authorize()]
	[Route("api/[controller]")]
	[ApiController]
	public class LessonsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public LessonsController(IUnitOfWork unitOfWork, ILogger<LessonsController> logger, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetLesson(int id)
		{
			try
			{
				var lesson = await _unitOfWork.LessonRepo.GetWithMulti(new List<Expression<Func<Lesson, bool>>> { c => c.Status == true, c=>c.Id == id });

				if(lesson == null && lesson.Count == 0)
				{
					return BadRequest("Error no lesson found");
				}
				var lessonsMap = _mapper.Map<LessonGetDTO>(lesson.FirstOrDefault());

				return Ok(lessonsMap);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"There was an error in {nameof(GetLesson)}");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("courselessons/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetLessonsCourse(int id)
		{
			if (id < 1)
			{
				return BadRequest("Invalid data");
			}

			try
			{
				var course = await _unitOfWork.CourseRepo.Get(c => c.Id == id);

				if (course == null)
				{
					return BadRequest("No course with this id found");
				}

				var lessons = await _unitOfWork.LessonRepo.GetWithMulti(new List<Expression<Func<Lesson, bool>>> { l => l.CourseId == course.Id, c=>c.Status == true }, c=>c.OrderBy(c=>c.Order));

				if(lessons == null)
				{
					return NoContent();
				}

				var mappedLessons = _mapper.Map<IList<LessonListDTO>>(lessons);

				return Ok(mappedLessons);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"There was an error in {nameof(GetLessonsCourse)}");
				return StatusCode(500, "Internal server error");
			}
		}

		//[HttpPost("{id:int}")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> CreateLesson(int id, [FromBody] CreateLessonDTO lessonDTO)
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
		//			return BadRequest("No course with this id found");
		//		}

		//		var lesson = _mapper.Map<Lesson>(lessonDTO);

		//		lesson.CourseId = course.Id;

		//		await _unitOfWork.LessonRepo.Create(lesson);

		//		await _unitOfWork.Save();

		//		return Ok(lesson);

		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"There was an error in {nameof(CreateLesson)}");
		//		return StatusCode(500, "Internal server error");
		//	}
		//}

		//[HttpPut("{id:int}")]
		//[ProducesResponseType(StatusCodes.Status204NoContent)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> UpdateLesson(int id, [FromBody] CreateLessonDTO lessonDTO)
		//{
		//	if(!ModelState.IsValid || id < 1)
		//	{
		//		return BadRequest("Invaid data");
		//	}

		//	try
		//	{
		//		var lesson = await _unitOfWork.LessonRepo.Get(l => l.Id == id);
		//		if(lesson == null)
		//		{
		//			return BadRequest("No lesson with this id found");
		//		}

		//		var lessonMap = _mapper.Map(lessonDTO, lesson);

		//		_unitOfWork.LessonRepo.Update(lessonMap);

		//		await _unitOfWork.Save();

		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"There was an error in {nameof(UpdateLesson)}");
		//		return StatusCode(500, "Internal server error");
		//	}

		//}

		//[HttpDelete("{id:int}")]
		//[ProducesResponseType(StatusCodes.Status204NoContent)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> DeleteLesson(int id)
		//{
		//	if (id < 1)
		//	{
		//		return BadRequest("Invaid data");
		//	}

		//	try
		//	{
		//		var lesson = await _unitOfWork.LessonRepo.Get(l => l.Id == id);

		//		if (lesson == null)
		//		{
		//			return BadRequest("No lesson with this id found");
		//		}
		//		lesson.Status = false;

		//		_unitOfWork.LessonRepo.Update(lesson);

		//		await _unitOfWork.Save();

		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"There was an error in {nameof(DeleteLesson)}");
		//		return StatusCode(500, "Internal server error");
		//	}

		//}

	}
}
