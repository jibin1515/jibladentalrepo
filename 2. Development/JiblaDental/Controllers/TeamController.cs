using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace JiblaDental.Controllers
{
	public class TeamController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Team> _teamRepo;
		private readonly IRepository<PageSettings> _pageRepo;
		private readonly IRepository<Department> _departmentRepo;
		private readonly IRepository<TeamContent> _teamContentRepo;

		public TeamController(IMapper mapper,
			IRepository<Team> teamRepo,
			IRepository<PageSettings> pageRepo,
			IRepository<Department> departmentRepo,
			IRepository<TeamContent> teamContentRepo)
		{
			_mapper = mapper;
			_teamRepo = teamRepo;
			_pageRepo = pageRepo;
			_departmentRepo = departmentRepo;
			_teamContentRepo = teamContentRepo;
		}

		[HttpGet("/team/{*url}")]
		public async Task<IActionResult> Index(string url)
		{
			var data = _mapper.Map<TeamDto>(await _teamRepo.FirstOrDefaultActive(x =>
			string.Equals(x.PageName, url, StringComparison.InvariantCultureIgnoreCase)));
			if (data == null)
			{
				return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					Teams = _mapper.Map<List<TeamDto>>(await _teamRepo.GetAllActive()),
					Departments = _mapper.Map<List<DepartmentDto>>(await _departmentRepo.GetAllActive()),
					IsDetails = false,
				});
			}
			var departmentIds = data.DepartmentIds.Split(',').Select(long.Parse).ToList();
			return View(
					new HomeViewModel
					{
						Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
						Team = data,
						IsDetails = true,
						TeamContent = _mapper.Map<List<TeamContentDto>>(await _teamContentRepo.WhereActive(x => x.TeamId == data.Id)),
						Departments = _mapper.Map<List<DepartmentDto>>(await _departmentRepo.GetAllActive()),
						Teams = _mapper.Map<List<TeamDto>>(await _teamRepo.WhereActive(x => x.DepartmentIds.Contains(departmentIds.FirstOrDefault().ToString()))),
						Department = _mapper.Map<DepartmentDto>(await _departmentRepo.Get(departmentIds.FirstOrDefault()))
					});
		}
		[HttpGet("/department/{*url}")]
		public async Task<IActionResult> Departments(string url)
		{
			var data = _mapper.Map<DepartmentDto>(await _departmentRepo.FirstOrDefaultActive(x =>
			string.Equals(x.PageName, url, StringComparison.InvariantCultureIgnoreCase)));
			if (data == null) return RedirectToAction("PageNotFound", "Home");
			
			return View(
					new HomeViewModel
					{
						Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
						Teams = _mapper.Map<List<TeamDto>>(await _teamRepo.WhereActive(x => x.DepartmentIds.Contains(data.Id.ToString()))),
						Departments = _mapper.Map<List<DepartmentDto>>(await _departmentRepo.GetAllActive()),
					});
		}
        [HttpPost("/team/search")]
        public async Task<IActionResult> Search(TeamDto model)
        {
			var department = _mapper.Map<List<TeamDto>>(await _teamRepo.GetAllActive());
			department = department.Where(x => x.NameEnglish.Contains(model.NameEnglish) || x.NameArabic.Contains(model.NameEnglish)).ToList();
            return View(
                new HomeViewModel
                {
                    Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
                    Teams = department,
                    Departments = _mapper.Map<List<DepartmentDto>>(await _departmentRepo.GetAllActive()),
                    IsDetails = false,
                });
        }
    }
}
