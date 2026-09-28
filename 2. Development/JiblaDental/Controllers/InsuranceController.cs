using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers
{
	public class InsuranceController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Insurance> _repo;
		private readonly IRepository<PageSettings> _pageRepo;
		private readonly IRepository<InsuranceSection> _sectionRepo;

		public InsuranceController(IMapper mapper, IRepository<Insurance> repo,
			IRepository<PageSettings> pageRepo, IRepository<InsuranceSection> sectionRepo)
		{
			_mapper = mapper;
			_repo = repo;
			_pageRepo = pageRepo;
			_sectionRepo = sectionRepo;
		}

		[HttpGet("/insurance")]
		public async Task<IActionResult> Index()
		{
			return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					Insurance = _mapper.Map<List<InsuranceDto>>(await _repo.GetAllActive()),
					InsuranceSection = _mapper.Map<InsuranceSectionDto>(await  _sectionRepo.FirstActive())
				});
		}
	}
}
