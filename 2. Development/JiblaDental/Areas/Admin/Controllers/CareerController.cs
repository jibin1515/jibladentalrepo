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
    public class CareerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<Career> _repo;
        private readonly IRepository<CareerSection> _sectionRepo;
		private readonly IRepository<CareerContent> _contentRepo;
        public CareerController(IMapper mapper,
            IFileService fileService,
            IRepository<Career> repo,
			IRepository<CareerSection> sectionRepo,
			IRepository<CareerContent> contentRepo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _repo = repo;
            _sectionRepo = sectionRepo;
			_contentRepo = contentRepo;
        }

		#region Section 
		[HttpGet("/admin/careers")]
		public async Task<IActionResult> GetSection()
		{
			return View(new CareerViewModel
			{
				Section = _mapper.Map<CareerSectionDto>(await _sectionRepo.First()),
				Careers = _mapper.Map<List<CareerDto>>(await _repo.GetAll())
			});
		}
		[HttpPost("/admin/career/section")]
		public async Task<IActionResult> PostSection(CareerSectionDto model)
		{
			var entity = await _sectionRepo.First();
			_mapper.Map(model, entity);
			await _sectionRepo.Update(entity);
			await _sectionRepo.SaveChanges();
			return RedirectToAction(nameof(GetSection));
		}
		#endregion

		#region career 
        [HttpGet("/admin/career/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            return View(id > 0
                ? new CareerViewModel
                {
                    Career = _mapper.Map<CareerDto>(await _repo.Get(id)),
                    CareersContent = _mapper.Map<List<CareerContentDto>>(await _contentRepo.Where(x => x.CareerId == id)),
                }
                : new CareerViewModel
                {
                    Career = new CareerDto
                    {
                        Id = 0,
                        IsActive = true,
                    },
                    CareersContent = new List<CareerContentDto>()
                });
        }
        [HttpPost("/admin/career")]
        public async Task<IActionResult> Post(CareerDto model)
        {
            if (string.IsNullOrEmpty(model.PageName))
                model.PageName = model.TitleEnglish.ToLower().Replace(" ", "-");
            Career entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<Career>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Career");
                var entities = await _repo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _repo.Add(entity);
            }
            else
            {
                entity = (await _repo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Career");
                _mapper.Map(model, entity);
                await _repo.Update(entity);
            }
            await _repo.SaveChanges();
            return RedirectToAction(nameof(Get), new { id = entity.Id });
        }
        [HttpPost("/admin/career/delete/{id:long}")]
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
		[HttpGet("/admin/career/content/{id:long}")]
		public async Task<IActionResult> GetContent(long id, [FromQuery] long? careerId)
		{
			return View(id > 0
				? _mapper.Map<CareerContentDto>(await _contentRepo.Get(id))
				: new CareerContentDto
				{
					IsActive = true,
					CareerId = careerId!.Value,
					Id = 0
				});
		}
		[HttpPost("/admin/career/content")]
		public async Task<IActionResult> PostContent(CareerContentDto model)
		{
			CareerContent entity;
			if (model.Id <= 0)
			{
				entity = _mapper.Map<CareerContent>(model);
				var entities = await _contentRepo.Where(x => x.CareerId == model.CareerId);
				entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
				await _contentRepo.Add(entity);
				await _contentRepo.SaveChanges();
			}
			else
			{
				entity = (await _contentRepo.Get(model.Id))!;
				_mapper.Map(model, entity);
				await _contentRepo.Update(entity);
				await _contentRepo.SaveChanges();
			}
			return RedirectToAction(nameof(GetContent), new { id = entity.Id });
		}
		[HttpPost("/admin/career/content/delete/{id:long}")]
		public async Task<IActionResult> DeleteContent(long id)
		{
			var entity = await _contentRepo.Get(id);
			if (entity != null)
			{
				await _fileService.DeleteAllFiles(entity);
				await _contentRepo.SoftDelete(entity);
				await _contentRepo.SaveChanges();
			}
			return RedirectToAction(nameof(Get), new { @id = entity.CareerId });
		}
		#endregion

	}
}
