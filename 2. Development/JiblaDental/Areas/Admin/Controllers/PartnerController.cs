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
    public class PartnerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<PartnerSection> _sectionRepo;
        private readonly IRepository<PartnerCategory> _repo;
        private readonly IRepository<Partner> _contentRepo;

        public PartnerController(IMapper mapper,
            IFileService fileService,
            IRepository<PartnerSection> sectionRepo,
            IRepository<Partner> contentRepo,
            IRepository<PartnerCategory> repo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _repo = repo;
            _sectionRepo = sectionRepo;
            _contentRepo = contentRepo;
        }

        #region Section 
        [HttpGet("/admin/partners")]
        public async Task<IActionResult> GetSection()
        {
            return View(new PartnerViewModel
            {
                Section = _mapper.Map<PartnerSectionDto>(await _sectionRepo.First()),
                PartnerCategorys = _mapper.Map<List<PartnerCategoryDto>>(await _repo.GetAll())
            });
        }
        [HttpPost("/admin/partners/section")]
        public async Task<IActionResult> PostSection(PartnerSectionDto model)
        {
            var entity = await _sectionRepo.First();
            await _fileService.SaveAllFiles(entity, model, "Uploads/Partner");
            _mapper.Map(model, entity);
            await _sectionRepo.Update(entity);
            await _sectionRepo.SaveChanges();
            return RedirectToAction(nameof(GetSection));
        }
        #endregion

        #region partnercategory
        [HttpGet("/admin/partner/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            return View(id > 0 ?
                new PartnerViewModel
                {
                    PartnerCategory = _mapper.Map<PartnerCategoryDto>(await _repo.Get(id)),
                    Partner = _mapper.Map<List<PartnerDto>>(await _contentRepo.Where(x => x.PartnerCategoryId == id)),
                }
                : new PartnerViewModel
                {
                    PartnerCategory = new PartnerCategoryDto { Id = 0, IsActive = true, },
                    Partner = new List<PartnerDto>()
                });
        }
        [HttpPost("/admin/partner")]
        public async Task<IActionResult> Post(PartnerCategoryDto model)
        {
            PartnerCategory entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<PartnerCategory>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Partner");
                var entities = await _repo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _repo.Add(entity);
            }
            else
            {
                entity = (await _repo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Partner");
                _mapper.Map(model, entity);
                await _repo.Update(entity);
            }
            await _repo.SaveChanges();
            return RedirectToAction(nameof(Get), new { id = entity.Id });
        }
        [HttpPost("/admin/partner/delete/{id:long}")]
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

        #region Content 
        [HttpGet("/admin/partner/content/{id:long}")]
        public async Task<IActionResult> GetContent(long id, [FromQuery] long? partnercategoryId)
        {
            return View(id > 0 ?
                _mapper.Map<PartnerDto>(await _contentRepo.Get(id))
                : new PartnerDto
                {
                    IsActive = true,
                    PartnerCategoryId = partnercategoryId!.Value,
                    Id = 0
                });
        }
        [HttpPost("/admin/partner/content")]
        public async Task<IActionResult> PostContent(PartnerDto model)
        {
            if (string.IsNullOrEmpty(model.PageName))
                model.PageName = model.TitleEnglish.ToLower().Replace(" ", "-");
            Partner entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<Partner>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Partner");
                var entities = await _contentRepo.Where(x => x.PartnerCategoryId == model.PartnerCategoryId);
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _contentRepo.Add(entity);
                await _contentRepo.SaveChanges();
            }
            else
            {
                entity = (await _contentRepo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Partner");
                _mapper.Map(model, entity);
                await _contentRepo.Update(entity);
                await _contentRepo.SaveChanges();
            }
            return RedirectToAction(nameof(GetContent), new { id = entity.Id });
        }
        [HttpPost("/admin/partner/content/delete/{id:long}")]
        public async Task<IActionResult> DeleteContent(long id)
        {
            var entity = await _contentRepo.Get(id);
            if (entity != null)
            {
                await _fileService.DeleteAllFiles(entity);
                await _contentRepo.SoftDelete(entity);
                await _contentRepo.SaveChanges();
            }
            return RedirectToAction(nameof(Get), new { @id = entity.PartnerCategoryId });
        }
        #endregion
    }
}
