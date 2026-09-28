using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers
{
	public class FaqController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Faq> _repo;
		private readonly IRepository<PageSettings> _pageRepo;

		public FaqController(IMapper mapper, IRepository<Faq> repo,
			IRepository<PageSettings> pageRepo)
		{
			_mapper = mapper;
			_repo = repo;
			_pageRepo = pageRepo;
		}

		[HttpGet("/faq")]
		public async Task<IActionResult> Index()
		{
			return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					Faq = _mapper.Map<List<FaqDto>>(await _repo.GetAllActive()),
				});
		}
	}
}
