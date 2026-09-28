using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JiblaDental.Controllers
{
	public class HomeController : Controller
	{
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<HomeBanner> _homeBannerRepo;
        private readonly IRepository<PageSettings> _pageRepo;
        private readonly IRepository<Service> _servicesRepo;
        private readonly IRepository<About> _aboutRepo;
        private readonly IRepository<ServiceSection> _serviceSectionRepo;
        private readonly IRepository<Team> _clientsRepo;
        private readonly IRepository<TeamSection> _clientSectionRepo;
        private readonly IRepository<PartnerSection> _partnerSectionRepo;
        private readonly IRepository<Partner> _partnerRepo;

        public HomeController(ILogger<HomeController> logger,
            IMapper mapper,
            IRepository<HomeBanner> homeBannerRepo,
            IRepository<PageSettings> pageRepo,
            IRepository<Service> servicesRepo,
            IRepository<About> aboutRepo,
            IRepository<ServiceSection> serviceSectionRepo,
            IRepository<Team> clientsRepo,
            IRepository<TeamSection> clientSectionRepo,
            IRepository<PartnerSection> partnerSectionRepo,
            IRepository<Partner> partnerRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _homeBannerRepo = homeBannerRepo;
            _pageRepo = pageRepo;
            _servicesRepo = servicesRepo;
            _aboutRepo = aboutRepo;
            _serviceSectionRepo = serviceSectionRepo;
            _clientsRepo = clientsRepo;
            _clientSectionRepo = clientSectionRepo;
            _partnerSectionRepo = partnerSectionRepo;
            _partnerRepo = partnerRepo;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            return View(
                new HomeViewModel
                {
                    Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
                    HomeBanner = _mapper.Map<List<HomeBannerDto>>(await _homeBannerRepo.GetAllActive()),
                    Services = _mapper.Map<List<ServiceDto>>(await _servicesRepo.WhereActive(x => x.ShowOnHomePage)),
                    About = _mapper.Map<AboutDto>(await _aboutRepo.First()),
                    ServiceSection = _mapper.Map<ServiceSectionDto>(await _serviceSectionRepo.First()),
                    TeamSection = _mapper.Map<TeamSectionDto>(await _clientSectionRepo.First()),
                    Teams = _mapper.Map<List<TeamDto>>(await _clientsRepo.GetAllActive()),
                    PartnerSection = _mapper.Map<PartnerSectionDto>(await _partnerSectionRepo.First()),
                    Partners = _mapper.Map<List<PartnerDto>>(await _partnerRepo.GetAllActive()),
                });
        }

        #region Common
        [HttpPost("/language")]
        public IActionResult Language(string code, string returnUrl)
        {
            Response.Cookies.Append("CultureCode", code);
            return Redirect(returnUrl);
        }

        [HttpGet("/result")]
        public async Task<IActionResult> Result()
        {
            var message = TempData["ResultMessage"] as string ??
                          "Thanks for contacting us! We will be in touch with you shortly.";
            var model = new HomeViewModel
            {
                Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
                Message = message
            };
            return View(model);
        }

        [HttpGet("/handle-error/{code:int}")]
        public IActionResult HandleError(int code)
        {
            return code switch
            {
                404 => RedirectToAction(nameof(PageNotFound)),
                _ => RedirectToAction(nameof(ServerError))
            };
        }

        [HttpGet("/page-not-found")]
        public async Task<IActionResult> PageNotFound()
        {
            return View("Error", new ErrorViewModel
            {
                Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAll()),
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = "Oops! This page doesn't exist."
            });
        }

        [HttpGet("/server-error")]
        public async Task<IActionResult> ServerError()
        {
            return View("Error", new ErrorViewModel
            {
                Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAll()),
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = "Oops! Something went wrong."
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
