//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using AutoMapper;
//using SpatulaApi.IRepository;
//using SpatulaApi.Models;
//using SpatulaApi.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace SpatulaApi.Controllers
//{
//	[Route("api/[controller]")]
//	[ApiController]
//	public class CountriesController : ControllerBase
//	{
//		private readonly IUnitOfWork _unitOfWork;

//		private readonly ILogger _logger;

//		private readonly IMapper _mapper;

//		private IAuthManager _authManager;

//		public CountriesController(IUnitOfWork unitOfWork, ILogger<CountriesController> logger, IMapper mapper,
//			IAuthManager authManager)
//		{
//			_unitOfWork = unitOfWork;
//			_logger = logger;
//			_mapper = mapper;
//			_authManager = authManager;
//		}
//		[HttpGet]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
//		public async Task<IActionResult> GetCountries()
//		{
//			try
//			{
//				var countries = await _unitOfWork.CountryRepo.GetAll();
//				var results = _mapper.Map<IList<CountryDTO>>(countries);
//				return Ok(results);
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, $"An error was caught in {nameof(GetCountries)}");
//				return StatusCode(500, "There was an error, Please try again later!");
//			}
//		}

//		[Authorize]
//		[HttpGet("{id:int}")]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
//		public async Task<IActionResult> GetCountry(int id)
//		{

//			_logger.LogInformation("Claim type name =" + HttpContext.User.Claims.Where(o => o.Type == ClaimTypes.PrimarySid).FirstOrDefault().Value);
			
//			try
//			{
//				var country = await _unitOfWork.CountryRepo.Get(c => c.Id == id, new List<string> { "Hotels" });
//				var result = _mapper.Map<CountryDTO>(country);
//				return Ok(result);
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, $"Error has occured in {nameof(GetCountry)}");
//				return StatusCode(500, "There was an error, Please try again later!");
//			}
//		}
//	}
//}
