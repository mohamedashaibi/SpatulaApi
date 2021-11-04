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

namespace SpatulaApi.Controllers.Api
{
	//[Authorize()]
	[Route("api/[controller]")]
	[ApiController]
	public class AdvertsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public AdvertsController(IUnitOfWork unitOfWork, ILogger<AdvertsController> logger, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
		}

		//[Authorize()]
		[HttpGet]
		public async Task<IActionResult> GetAllAdverts()
		{
			try
			{
				var adverts = await _unitOfWork.AdvertRepo.GetAll(c => c.Status == true);
				return Ok(adverts);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"There was an error in {nameof(GetAllAdverts)}");
				return StatusCode(500, "We have encountered a problem, please try again later.");
			}
		}
		//[Authorize()]
		[HttpGet("{id:int}", Name = "GetAdvert")]
		public async Task<IActionResult> GetAdvert(int id)
		{
			try
			{
				var advert = await _unitOfWork.AdvertRepo.GetWithMulti(new List<Expression<Func<Advert, bool>>> { c => c.Id == id, c => c.Status == true });

				if (advert == null)
				{
					return BadRequest("Error in category");
				}

				return Ok(advert.FirstOrDefault());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"There was a problem in {nameof(GetAdvert)}");
				return StatusCode(500, "Internal server error.");
			}
		}
	}
}
