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
    public class ServiceController : Controller
    {
        private readonly IMapper _mapper; 
        private readonly IFileService _fileService; 
        private readonly IRepository<ServiceSection> _sectionRepo; 
        private readonly IRepository<Service> _repo; 
        public ServiceController(IMapper mapper, 
            IFileService fileService, 
            IRepository<ServiceSection> sectionRepo, 
            IRepository<Service> repo) 
        { 
            _mapper = mapper; 
            _fileService = fileService; 
            _repo = repo; 
            _sectionRepo = sectionRepo; 
        }
        
        #region Section 
        [HttpGet("/admin/services")]
        public async Task<IActionResult> GetSection() 
        { 
            return View(new ServiceViewModel 
            { 
                Section = _mapper.Map<ServiceSectionDto>(await _sectionRepo.First()), 
                Services = _mapper.Map<List<ServiceDto>>(await _repo.GetAll())
            }); 
        }
        [HttpPost("/admin/service-section")]
        public async Task<IActionResult> PostSection(ServiceSectionDto model) 
        { 
            var entity = await _sectionRepo.First(); 
            await _fileService.SaveAllFiles(entity, model, "Uploads/Service"); 
            _mapper.Map(model, entity); 
            await _sectionRepo.Update(entity); 
            await _sectionRepo.SaveChanges(); 
            return RedirectToAction(nameof(GetSection)); 
        }
        #endregion

        #region service 
        [HttpGet("/admin/service/{id:long}")]
        public async Task<IActionResult> Get(long id) 
        { 
            return View(id > 0 ? 
                _mapper.Map<ServiceDto>(await _repo.Get(id)) 
                : new ServiceDto 
                { 
                    IsActive = true, 
                    Id = 0 
                }); 
        }
        [HttpPost("/admin/service")]
        public async Task<IActionResult> Post(ServiceDto model) 
        { 
            if (string.IsNullOrEmpty(model.PageName)) 
                model.PageName = model.TitleEnglish.ToLower().Replace(" ", "-"); 
            Service entity; 
            if (model.Id <= 0) 
            { 
                entity = _mapper.Map<Service>(model); 
                await _fileService.SaveAllFiles(entity, model, "Uploads/Service"); 
                var entities = await _repo.GetAll(); 
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1; 
                await _repo.Add(entity); 
            } 
            else 
            { 
                entity = (await _repo.Get(model.Id))!; 
                await _fileService.SaveAllFiles(entity, model, "Uploads/Service"); 
                _mapper.Map(model, entity); 
                await _repo.Update(entity); 
            } 
            await _repo.SaveChanges(); 
            return RedirectToAction(nameof(Get), new { id = entity.Id }); 
        }
        [HttpPost("/admin/service/delete/{id:long}")]
        public async Task<IActionResult> Delete(long id) 
        { 
            var entity = await _repo.Get(id); 
            if (entity != null) 
            { 
                await _fileService.DeleteAllFiles(entity); 
                await _repo.SoftDelete(entity); 
                await _repo.SaveChanges(); 
            } 
            return RedirectToAction(nameof(GetSection)); 
        }
        #endregion
    }
}
