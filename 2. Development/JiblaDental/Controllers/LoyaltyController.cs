using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers
{
	public class LoyaltyController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Loyalty> _repo;
		private readonly IRepository<PageSettings> _pageRepo;
		private readonly IRepository<LoyaltyImage> _imagesRepo;

		public LoyaltyController(IMapper mapper,
			IRepository<Loyalty> repo,
			IRepository<PageSettings> pageRepo,
			IRepository<LoyaltyImage> imagesRepo)
		{
			_mapper = mapper;
			_repo = repo;
			_pageRepo = pageRepo;
			_imagesRepo = imagesRepo;
		}

		[HttpGet("/loyalty-program/{*url}")]
		public async Task<IActionResult> Index(string url)
		{
			var data = _mapper.Map<LoyaltyDto>(await _repo.FirstOrDefaultActive(x =>
			string.Equals(x.PageName, url, StringComparison.InvariantCultureIgnoreCase)));
			if (data == null)
			{
				return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					Loyalties = _mapper.Map<List<LoyaltyDto>>(await _repo.GetAllActive()),
					IsDetails = false,
				});
			}
			return View(
					new HomeViewModel
					{
						Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
						Loyalty = data,
						IsDetails = true,
						LoyaltyImages = _mapper.Map<List<LoyaltyImageDto>>(await _imagesRepo.WhereActive(x => x.LoyaltyId == data.Id))
					});
		}
	}
}
