using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<TeamSection> _sectionRepo;
        private readonly IRepository<Department> _repo;
        private readonly IRepository<Team> _contentRepo;
        private readonly IRepository<TeamContent> _teamContentRepo;

        public TeamController(IMapper mapper, IFileService fileService,
            IRepository<TeamSection> sectionRepo, IRepository<Department> repo,
            IRepository<Team> contentRepo, IRepository<TeamContent> teamContentRepo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _sectionRepo = sectionRepo;
            _repo = repo;
            _contentRepo = contentRepo;
            _teamContentRepo = teamContentRepo;
        }

        #region Section 
        [HttpGet("/admin/team")]
        public async Task<IActionResult> GetSection()
        {
            return View(new TeamViewModel
            {
                Section = _mapper.Map<TeamSectionDto>(await _sectionRepo.First()),
                Departments = _mapper.Map<List<DepartmentDto>>(await _repo.GetAll())
            });
        }
        [HttpPost("/admin/team/section")]
        public async Task<IActionResult> PostSection(TeamSectionDto model)
        {
            var entity = await _sectionRepo.First();
            await _fileService.SaveAllFiles(entity, model, "Uploads/Department");
            _mapper.Map(model, entity);
            await _sectionRepo.Update(entity);
            await _sectionRepo.SaveChanges(); return RedirectToAction(nameof(GetSection));
        }
        #endregion

        #region department 
        [HttpGet("/admin/team/department/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            return View(id > 0 ?
                new TeamViewModel
                {
                    Department = _mapper.Map<DepartmentDto>(await _repo.Get(id)),
                    Teams = _mapper.Map<List<TeamDto>>(await _contentRepo.Where(x => x.DepartmentId == id)),
                }
                : new TeamViewModel
                {
                    Department = new DepartmentDto { Id = 0, IsActive = true, },
                    Teams = new List<TeamDto>()
                });
        }
        [HttpPost("/admin/team/department")]
        public async Task<IActionResult> Post(DepartmentDto model)
        {
            if (string.IsNullOrEmpty(model.PageName))
                model.PageName = model.TitleEnglish.ToLower().Replace(" ", "-");
            Department entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<Department>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Department");
                var entities = await _repo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _repo.Add(entity);
            }
            else
            {
                entity = (await _repo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Department");
                _mapper.Map(model, entity);
                await _repo.Update(entity);
            }
            await _repo.SaveChanges();
            return RedirectToAction(nameof(Get), new { id = entity.Id });
        }
        [HttpPost("/admin/team/department/delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _repo.Get(id);
            if (entity != null)
            {
                await _fileService.DeleteAllFiles(entity);
                await _repo.SoftDelete(entity); await _repo.SaveChanges();
            }
            return RedirectToAction(nameof(GetSection));
        }
        #endregion

        #region Doctor 
        [HttpGet("/admin/doctor")]
        public async Task<IActionResult> GetAll()
        {
            return View(_mapper.Map<List<TeamDto>>(await _contentRepo.GetAll()));
        }
        [HttpGet("/admin/team/doctor/{id:long}")]
        public async Task<IActionResult> GetContent(long id)
        {
            return View(id > 0 ?
                new TeamViewModel
                {
                    Team = _mapper.Map<TeamDto>(await _contentRepo.Get(id)),
                    Content = _mapper.Map<List<TeamContentDto>>(await _teamContentRepo.Where(x => x.TeamId == id)),
                    Departments = _mapper.Map<List<DepartmentDto>>(await _repo.GetAll())
                }
                : new TeamViewModel
                {
                    Departments = _mapper.Map<List<DepartmentDto>>(await _repo.GetAll()),
                    Team = new TeamDto
                    {
                        IsActive = true,
                        Id = 0
                    },
                    Content = new List<TeamContentDto>()
                });
        }
        [HttpPost("/admin/team/doctor")]
        public async Task<IActionResult> PostContent(TeamDto model)
        {
            if (string.IsNullOrEmpty(model.PageName))
                model.PageName = model.NameEnglish.ToLower().Replace(" ", "-");
            Team entity;
            model.DepartmentIds = string.Join(",", model.DepId);
            if (model.Id <= 0)
            {
                entity = _mapper.Map<Team>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Department");
                var entities = await _contentRepo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _contentRepo.Add(entity);
                await _contentRepo.SaveChanges();
            }
            else
            {
                entity = (await _contentRepo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Department");
                _mapper.Map(model, entity); await _contentRepo.Update(entity);
                await _contentRepo.SaveChanges();
            }
            return RedirectToAction(nameof(GetContent), new { id = entity.Id });
        }
        [HttpPost("/admin/team/doctor/delete/{id:long}")]
        public async Task<IActionResult> DeleteContent(long id)
        {
            var entity = await _contentRepo.Get(id);
            if (entity != null)
            {
                await _fileService.DeleteAllFiles(entity);
                await _contentRepo.SoftDelete(entity);
                await _contentRepo.SaveChanges();
            }
            return RedirectToAction(nameof(Get), new { @id = entity.DepartmentId });
        }
        #endregion

        #region Content 
        [HttpGet("/admin/doctor/service/{id:long}")]
        public async Task<IActionResult> GetService(long id, [FromQuery] long? teamId, [FromQuery] string? type)
        {
            return View(id > 0 ?
                _mapper.Map<TeamContentDto>(await _teamContentRepo.Get(id))
                : new TeamContentDto
                {
                    IsActive = true,
                    TeamId = teamId!.Value,
                    Id = 0,
                    Type = type
                });
        }
        [HttpPost("/admin/doctor/service")]
        public async Task<IActionResult> PostService(TeamContentDto model)
        {
            TeamContent entity;
            if (model.Type == "social") model.Name = model.Name.ToLower();
            if (model.Id <= 0)
            {
                entity = _mapper.Map<TeamContent>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Department");
                var entities = await _teamContentRepo.Where(x => x.TeamId == model.TeamId && x.Type == model.Type);
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _teamContentRepo.Add(entity);
                await _teamContentRepo.SaveChanges();
            }
            else
            {
                entity = (await _teamContentRepo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Department");
                _mapper.Map(model, entity); await _teamContentRepo.Update(entity);
                await _teamContentRepo.SaveChanges();

            }
            return RedirectToAction(nameof(GetService), new { id = entity.Id });
        }
        [HttpPost("/admin/doctor/service/delete/{id:long}")]
        public async Task<IActionResult> DeleteService(long id)
        {
            var entity = await _teamContentRepo.Get(id);
            if (entity != null)
            {
                await _fileService.DeleteAllFiles(entity);
                await _teamContentRepo.SoftDelete(entity);
                await _teamContentRepo.SaveChanges();
            }
            return RedirectToAction(nameof(GetContent), new { @id = entity.TeamId });
        }
        #endregion
    }
}
