using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers
{
	public class PolicyController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Policy> _repo;
		private readonly IRepository<PageSettings> _pageRepo;

		public PolicyController(IMapper mapper, IRepository<Policy> repo,
			IRepository<PageSettings> pageRepo)
		{
			_mapper = mapper;
			_repo = repo;
			_pageRepo = pageRepo;
		}

		[HttpGet("/privacy-policy")]
		public async Task<IActionResult> Index()
		{
			return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					Policy = _mapper.Map<List<PolicyDto>>(await _repo.GetAllActive()),
				});
		}
	}
}
