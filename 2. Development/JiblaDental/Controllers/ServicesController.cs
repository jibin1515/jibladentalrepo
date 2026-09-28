using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers
{
	public class ServicesController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Service> _servicesRepo;
		private readonly IRepository<PageSettings> _pageRepo;
		private readonly IRepository<ServiceSection> _serviceSectionRepo;
		private readonly IRepository<Contact> _contactRepo;

		public ServicesController(IMapper mapper,
			IRepository<Service> servicesRepo,
			IRepository<PageSettings> pageRepo,
			IRepository<ServiceSection> serviceSectionRepo,
			IRepository<Contact> contactRepo)
		{
			_mapper = mapper;
			_servicesRepo = servicesRepo;
			_pageRepo = pageRepo;
			_serviceSectionRepo = serviceSectionRepo;
			_contactRepo = contactRepo;
		}

		[HttpGet("/services/{*url}")]
		public async Task<IActionResult> Index(string url)
		{
			var data = _mapper.Map<ServiceDto>(await _servicesRepo.FirstOrDefaultActive(x =>
			string.Equals(x.PageName, url, StringComparison.InvariantCultureIgnoreCase)));
			if (data == null)
			{
				return View(
				new HomeViewModel
				{
						Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
						Services = _mapper.Map<List<ServiceDto>>(await _servicesRepo.GetAllActive()),
						IsDetails = false,
					});
			}
			return View(
					new HomeViewModel
					{
						Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
						Service = data,
						IsDetails = true,
						Services = _mapper.Map<List<ServiceDto>>(await _servicesRepo.GetAllActive()),
						ServiceSection = _mapper.Map<ServiceSectionDto>(await _serviceSectionRepo.First()),
						Contact = _mapper.Map<ContactDto>(await _contactRepo.First()),
					});
		}
	}
}
