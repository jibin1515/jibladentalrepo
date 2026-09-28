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
    public class VideoHubController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<VideoHub> _repo;
        public VideoHubController(IMapper mapper,
            IFileService fileService,
            IRepository<VideoHub> repo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _repo = repo;

        }
        #region videohub 
        [HttpGet("/admin/video-hub")]
        public async Task<IActionResult> GetAll()
        {
            return View(_mapper.Map<List<VideoHubDto>>(await _repo.GetAll()));
        }
        [HttpGet("/admin/video-hub/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            return View(id > 0 ?
                _mapper.Map<VideoHubDto>(await _repo.Get(id))
                : new VideoHubDto
                {
                    IsActive = true,
                    Id = 0
                });
        }
        [HttpPost("/admin/video-hub")]
        public async Task<IActionResult> Post(VideoHubDto model)
        {
            VideoHub entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<VideoHub>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/VideoHub");
                var entities = await _repo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _repo.Add(entity);
            }
            else
            {
                entity = (await _repo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/VideoHub");
                _mapper.Map(model, entity);
                await _repo.Update(entity);
            }
            await _repo.SaveChanges();
            return RedirectToAction(nameof(Get), new { id = entity.Id });
        }
        [HttpPost("/admin/video-hub/delete/{id:long}")]
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
