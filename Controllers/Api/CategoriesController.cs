using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
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
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public CategoriesController(IUnitOfWork unitOfWork, ILogger<CategoriesController> logger, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
		}

		[Authorize()]
		[HttpGet]
		public async Task<IActionResult> GetAllCategories()
		{
			try
			{
				var categories = await _unitOfWork.CategoryRepo.GetAll(c=>c.Status == true);
				var result = _mapper.Map<IList<CategoryDTO>>(categories);
				return Ok(result);
			} catch (Exception ex)
			{
				_logger.LogError(ex, $"There was an error in {nameof(GetAllCategories)}");
				return StatusCode(500, "We have encountered a problem, please try again later.");
			}
		}
		[Authorize()]
		[HttpGet("{id:int}", Name = "GetCategory")]
		public async Task<IActionResult> GetCategory(int id)
		{
			try
			{
				var category = await _unitOfWork.CategoryRepo.GetWithMulti(new List<Expression<Func<Category, bool>>> { c => c.Id == id, c=>c.Status == true });
				
				if(category == null)
				{
					return BadRequest("Error in category");
				}
				
				var result = _mapper.Map<CategoryDTO>(category);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"There was a problem in {nameof(GetCategory)}");
				return StatusCode(500, "Internal server error.");
			}
		}
		//[Authorize(Roles = "Administrator")]
		//[HttpPost]
		//[ProducesResponseType(StatusCodes.Status201Created)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO categoryDTO)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		_logger.LogError("Error in Modelstate of creating category");
		//		return BadRequest(ModelState);
		//	}
		//	try
		//	{
		//		var category = _mapper.Map<Category>(categoryDTO);
		//		await _unitOfWork.CategoryRepo.Create(category);
		//		await _unitOfWork.Save();

		//		var catMap = _mapper.Map<CreateCategoryDTO>(category);

		//		return CreatedAtRoute("GetCategory", new { id = category.Id }, catMap);
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"There was a problem in {nameof(CreateCategory)}");
		//		return StatusCode(500, "Internal server error.");
		//	}

		//}

		//[Authorize(Roles = "Administrator")]
		//[HttpPut("{id:int}")]
		//[ProducesResponseType(StatusCodes.Status204NoContent)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDTO categoryDTO)
		//{
		//	if(!ModelState.IsValid || id < 1)
		//	{
		//		_logger.LogError("Error in updating category");
		//		return BadRequest("Invalid data");
		//	}

		//	var category = await _unitOfWork.CategoryRepo.Get(c => c.Id == id);

		//	if(category == null)
		//	{
		//		return BadRequest("Error, no object found with that id");
		//	}

		//	try
		//	{
		//		var catMap = _mapper.Map(categoryDTO, category);

		//		_unitOfWork.CategoryRepo.Update(catMap);

		//		await _unitOfWork.Save();

		//		return NoContent();

		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"There was a problem in {nameof(UpdateCategory)}");
		//		return StatusCode(500, "Internal server error.");
		//	}

		//}

		//[Authorize(Roles = "Administrator")]
		//[HttpDelete("{id:int}")]
		//[ProducesResponseType(StatusCodes.Status204NoContent)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> DeleteCategory(int id)
		//{



		//	if(id < 1)
		//	{
		//		_logger.LogError("Invalid data sent to deletecategory");
		//		return BadRequest("Invalid data");
		//	}

		//	var category = await _unitOfWork.CategoryRepo.Get(c => c.Id == id);

		//	if(category == null)
		//	{
		//		return BadRequest("No category with the id was found");
		//	}
		//	category.Status = false;

		//	try
		//	{
		//		_unitOfWork.CategoryRepo.Update(category);
		//		await _unitOfWork.Save();

		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, $"There was a problem in {nameof(DeleteCategory)}");
		//		return StatusCode(500, "Internal server error.");
		//	}
		//}

	}
}
