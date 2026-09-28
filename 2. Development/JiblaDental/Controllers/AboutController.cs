using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers
{
	public class AboutController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<About> _repo;
		private readonly IRepository<PageSettings> _pageRepo;
		private readonly IRepository<AboutGallery> _galleryRepo;
		private readonly IRepository<Testimonial> _testimonialRepo;

		public AboutController(IMapper mapper, IRepository<About> repo, 
			IRepository<PageSettings> pageRepo, IRepository<AboutGallery> galleryRepo, 
			IRepository<Testimonial> testimonialRepo)
		{
			_mapper = mapper;
			_repo = repo;
			_pageRepo = pageRepo;
			_galleryRepo = galleryRepo;
			_testimonialRepo = testimonialRepo;
		}

		[HttpGet("/our-center")]
		public async Task<IActionResult> Index()
		{
			return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					About = _mapper.Map<AboutDto>(await _repo.First()),
					AboutGalleries = _mapper.Map<List<AboutGalleryDto>>(await _galleryRepo.GetAllActive()),
					Testimonials = _mapper.Map<List<TestimonialDto>>(await _testimonialRepo.GetAllActive()),
				});
		}
	}
}
