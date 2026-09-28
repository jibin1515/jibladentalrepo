using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JiblaDental.Controllers
{
    public class NewsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepository<News> _repo;
        private readonly IRepository<PageSettings> _pageRepo;
        private readonly IRepository<NewsImage> _newsImageRepo;

        public NewsController(IMapper mapper, IRepository<News> repo,
            IRepository<PageSettings> pageRepo, IRepository<NewsImage> newsImageRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _pageRepo = pageRepo;
            _newsImageRepo = newsImageRepo;
        }

        [HttpGet("/news/{*url}")]
        public async Task<IActionResult> Index(string url)
        {
            var data = _mapper.Map<NewsDto>(await _repo.FirstOrDefaultActive(x =>
            string.Equals(x.PageName, url, StringComparison.InvariantCultureIgnoreCase)));
            if (data == null)
            {
                return View(
                new HomeViewModel
                {
                    Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
                    NewsList = _mapper.Map<List<NewsDto>>(await _repo.GetAllActive()),
                    IsDetails = false,
                });
            }
            return View(
                    new HomeViewModel
                    {
                        Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
                        News = data,
                        IsDetails = true,
						NewsImages = _mapper.Map<List<NewsImageDto>>(await _newsImageRepo.WhereActive(x => x.NewsId == data.Id)),
					});
        }
    }
}
