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
    public class PolicyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<Policy> _repo;
        public PolicyController(IMapper mapper,
            IFileService fileService,
            IRepository<Policy> repo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _repo = repo;
        }
        #region policy 
        [HttpGet("/admin/policy")]
        public async Task<IActionResult> GetAll()
        {
            return View(_mapper.Map<List<PolicyDto>>(await _repo.GetAll()));
        }
        [HttpGet("/admin/policy/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            return View(id > 0 ?
                _mapper.Map<PolicyDto>(await _repo.Get(id))
                : new PolicyDto
                {
                    IsActive = true,
                    Id = 0
                });
        }
        [HttpPost("/admin/policy")]
        public async Task<IActionResult> Post(PolicyDto model)
        {
            Policy entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<Policy>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Policy");
                var entities = await _repo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _repo.Add(entity);
            }
            else
            {
                entity = (await _repo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Policy");
                _mapper.Map(model, entity); await _repo.Update(entity);
            }
            await _repo.SaveChanges();
            return RedirectToAction(nameof(Get), new { id = entity.Id });
        }
        [HttpPost("/admin/policy/delete/{id:long}")]
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
