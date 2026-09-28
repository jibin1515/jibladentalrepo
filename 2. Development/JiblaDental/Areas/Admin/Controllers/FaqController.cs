using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class FaqController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<Faq> _repo;
        public FaqController(IMapper mapper,
            IFileService fileService,
            IRepository<Faq> repo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _repo = repo;
        }

        #region faq 
        [HttpGet("/admin/faq")]
        public async Task<IActionResult> GetAll()
        {
            return View(_mapper.Map<List<FaqDto>>(await _repo.GetAll()));
        }
        [HttpGet("/admin/faq/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            return View(id > 0
                ? _mapper.Map<FaqDto>(await _repo.Get(id))
                : new FaqDto
                {
                    IsActive = true,
                    Id = 0
                });
        }
        [HttpPost("/admin/faq")]
        public async Task<IActionResult> Post(FaqDto model)
        {
            Faq entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<Faq>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Faq");
                var entities = await _repo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _repo.Add(entity);
            }
            else
            {
                entity = (await _repo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Faq");
                _mapper.Map(model, entity);
                await _repo.Update(entity);
            }
            await _repo.SaveChanges();
            return RedirectToAction(nameof(Get), new { id = entity.Id });
        }
        [HttpPost("/admin/faq/delete/{id:long}")]
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
    }
}
