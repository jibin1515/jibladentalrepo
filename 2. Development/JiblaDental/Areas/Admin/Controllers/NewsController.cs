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
    public class NewsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<News> _repo;
        private readonly IRepository<NewsImage> _contentRepo;
        public NewsController(IMapper mapper,
            IFileService fileService,
            IRepository<NewsImage> contentRepo,
            IRepository<News> repo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _repo = repo;
            _contentRepo = contentRepo;
        }
        #region news 
        [HttpGet("/admin/smile-blogs")]
        public async Task<IActionResult> GetAll() 
        { 
            return View(_mapper.Map<List<NewsDto>>(await _repo.GetAll()));
        }
        [HttpGet("/admin/smile-blogs/{id:long}")]
        public async Task<IActionResult> Get(long id) 
        { 
            return View(id > 0 
                ? new NewsViewModel 
                { 
                    News = _mapper.Map<NewsDto>(await _repo.Get(id)), 
                    NewsImages = _mapper.Map<List<NewsImageDto>>(await _contentRepo.Where(x => x.NewsId == id)), 
                } 
                : new NewsViewModel 
                { 
                    News = new NewsDto 
                    { 
                        Id = 0, 
                        IsActive = true, 
                    }, 
                    NewsImages = new List<NewsImageDto>() 
                }); 
        }
        [HttpPost("/admin/smile-blogs")]
        public async Task<IActionResult> Post(NewsDto model) 
        { 
            if (string.IsNullOrEmpty(model.PageName)) 
                model.PageName = model.TitleEnglish.ToLower().Replace(" ", "-"); 
            News entity; 
            if (model.Id <= 0) 
            { 
                entity = _mapper.Map<News>(model); 
                await _fileService.SaveAllFiles(entity, model, "Uploads/News"); 
                var entities = await _repo.GetAll(); 
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1; 
                await _repo.Add(entity); 
            } 
            else 
            { 
                entity = (await _repo.Get(model.Id))!; 
                await _fileService.SaveAllFiles(entity, model, "Uploads/News"); 
                _mapper.Map(model, entity); 
                await _repo.Update(entity); 
            } 
            await _repo.SaveChanges(); 
            return RedirectToAction(nameof(Get), new { id = entity.Id }); 
        }
        [HttpPost("/admin/smile-blogs/delete/{id:long}")]
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
        [HttpGet("/admin/smile-blogs/content/{id:long}")]
        public async Task<IActionResult> GetContent(long id, [FromQuery] long? newsId) 
        { 
            return View(id > 0 
                ? _mapper.Map<NewsImageDto>(await _contentRepo.Get(id)) 
                : new NewsImageDto 
                { 
                    IsActive = true, 
                    NewsId = newsId!.Value, 
                    Id = 0
                }); 
        }
        [HttpPost("/admin/smile-blogs/content")]
        public async Task<IActionResult> PostContent(NewsImageDto model)
        {
            NewsImage entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<NewsImage>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/News");
                var entities = await _contentRepo.Where(x => x.NewsId == model.NewsId);
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _contentRepo.Add(entity);
                await _contentRepo.SaveChanges();
            }
            else
            {
                entity = (await _contentRepo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/News");
                _mapper.Map(model, entity);
                await _contentRepo.Update(entity);
                await _contentRepo.SaveChanges();                
            }
            return RedirectToAction(nameof(GetContent), new { id = entity.Id });
        }
        [HttpPost("/admin/smile-blogs/content/delete/{id:long}")]
        public async Task<IActionResult> DeleteContent(long id) 
        { 
            var entity = await _contentRepo.Get(id); 
            if (entity != null) 
            { 
                await _fileService.DeleteAllFiles(entity); 
                await _contentRepo.SoftDelete(entity); 
                await _contentRepo.SaveChanges(); 
            } 
            return RedirectToAction(nameof(Get), new { @id = entity.NewsId }); 
        } 
        #endregion
    }
}
