using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers
{
	public class VideoHubController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<VideoHub> _repo;
		private readonly IRepository<PageSettings> _pageRepo;

		public VideoHubController(IMapper mapper, IRepository<VideoHub> repo,
			IRepository<PageSettings> pageRepo)
		{
			_mapper = mapper;
			_repo = repo;
			_pageRepo = pageRepo;
		}

		[HttpGet("/video-hub")]
		public async Task<IActionResult> Index()
		{
			return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					VideoHubs = _mapper.Map<List<VideoHubDto>>(await _repo.GetAllActive()),
				});
		}
	}
}
