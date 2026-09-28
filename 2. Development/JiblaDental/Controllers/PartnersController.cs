using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers
{
	public class PartnersController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Contact> _contactRepo;
		private readonly IRepository<PartnerSection> _sectionRepo;
		private readonly IRepository<Partner> _repo;
		private readonly IRepository<PageSettings> _pageRepo;
		private readonly IRepository<PartnerCategory> _categoryRepo;

		public PartnersController(IMapper mapper, IRepository<Partner> repo,
			IRepository<PageSettings> pageRepo, IRepository<Contact> contactRepo,
			IRepository<PartnerSection> sectionRepo, IRepository<PartnerCategory> categoryRepo)
		{
			_mapper = mapper;
			_repo = repo;
			_pageRepo = pageRepo;
			_contactRepo = contactRepo;
			_sectionRepo = sectionRepo;
			_categoryRepo = categoryRepo;
		}

		[HttpGet("/partners")]
		public async Task<IActionResult> Index()
		{
			return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					PartnerSection = _mapper.Map<PartnerSectionDto>(await _sectionRepo.First()),
					Partners = _mapper.Map<List<PartnerDto>>(await _repo.GetAllActive()),
					Contact = _mapper.Map<ContactDto>(await _contactRepo.First()),
					PartnerCategories = _mapper.Map<List<PartnerCategoryDto>>(await _categoryRepo.GetAllActive()),
				});
		}
		[HttpGet("/partner/{id:long}")]
		public async Task<IActionResult> Details(long id)
		{
			var data = _mapper.Map<PartnerDto>(await _repo.Get(id));
			if (data == null) return RedirectToAction("PageNotFound", "Home");
			return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					PartnerSection = _mapper.Map<PartnerSectionDto>(await _sectionRepo.First()),
					Partners = _mapper.Map<List<PartnerDto>>(await _repo.GetAllActive()),
					Partner = data,
					Contact = _mapper.Map<ContactDto>(await _contactRepo.First()),
					PartnerCategories = _mapper.Map<List<PartnerCategoryDto>>(await _categoryRepo.GetAllActive()),
				});
		}
	}
}
