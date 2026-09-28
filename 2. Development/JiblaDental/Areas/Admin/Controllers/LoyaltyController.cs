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
    public class LoyaltyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<Loyalty> _repo;
        private readonly IRepository<LoyaltyImage> _contentRepo;
        public LoyaltyController(IMapper mapper,
            IFileService fileService,
            IRepository<LoyaltyImage> contentRepo,
            IRepository<Loyalty> repo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _repo = repo;
            _contentRepo = contentRepo;
        }
        #region Loyalty 
        [HttpGet("/admin/loyalty")]
        public async Task<IActionResult> GetAll()
        {
            return View(_mapper.Map<List<LoyaltyDto>>(await _repo.GetAll()));
        }
        [HttpGet("/admin/loyalty/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            return View(id > 0
                ? new LoyaltyViewModel
                {
                    Loyalty = _mapper.Map<LoyaltyDto>(await _repo.Get(id)),
                    LoyaltyImages = _mapper.Map<List<LoyaltyImageDto>>(await _contentRepo.Where(x => x.LoyaltyId == id)),
                }
                : new LoyaltyViewModel
                {
                    Loyalty = new LoyaltyDto
                    {
                        Id = 0,
                        IsActive = true,
                    },
                    LoyaltyImages = new List<LoyaltyImageDto>()
                });
        }
        [HttpPost("/admin/loyalty")]
        public async Task<IActionResult> Post(LoyaltyDto model)
        {
            if (string.IsNullOrEmpty(model.PageName))
                model.PageName = model.TitleEnglish.ToLower().Replace(" ", "-");
            Loyalty entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<Loyalty>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Loyalty");
                var entities = await _repo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _repo.Add(entity);
            }
            else
            {
                entity = (await _repo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Loyalty");
                _mapper.Map(model, entity);
                await _repo.Update(entity);
            }
            await _repo.SaveChanges();
            return RedirectToAction(nameof(Get), new { id = entity.Id });
        }
        [HttpPost("/admin/loyalty/delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _repo.Get(id);
            if (entity != null)
            {
                await _fileService.DeleteAllFiles(entity);
                await _repo.SoftDelete(entity);
                await _repo.SaveChanges();
            }
            return RedirectToAction(nameof(GetAll));
        }
        #endregion

        #region Content
        [HttpGet("/admin/loyalty/content/{id:long}")]
        public async Task<IActionResult> GetContent(long id, [FromQuery] long? loyaltyId)
        {
            return View(id > 0
                ? _mapper.Map<LoyaltyImageDto>(await _contentRepo.Get(id))
                : new LoyaltyImageDto
                {
                    IsActive = true,
                    LoyaltyId = loyaltyId!.Value,
                    Id = 0
                });
        }
        [HttpPost("/admin/loyalty/content")]
        public async Task<IActionResult> PostContent(LoyaltyImageDto model)
        {
            LoyaltyImage entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<LoyaltyImage>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Loyalty");
                var entities = await _contentRepo.Where(x => x.LoyaltyId == model.LoyaltyId);
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _contentRepo.Add(entity);
                await _contentRepo.SaveChanges();
            }
            else
            {
                entity = (await _contentRepo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Loyalty");
                _mapper.Map(model, entity);
                await _contentRepo.Update(entity);
                await _contentRepo.SaveChanges();
            }
            return RedirectToAction(nameof(GetContent), new { id = entity.Id });
        }
        [HttpPost("/admin/loyalty/content/delete/{id:long}")]
        public async Task<IActionResult> DeleteContent(long id)
        {
            var entity = await _contentRepo.Get(id);
            if (entity != null)
            {
                await _fileService.DeleteAllFiles(entity);
                await _contentRepo.SoftDelete(entity);
                await _contentRepo.SaveChanges();
            }
            return RedirectToAction(nameof(Get), new { @id = entity.LoyaltyId });
        }
        #endregion
    }
}
