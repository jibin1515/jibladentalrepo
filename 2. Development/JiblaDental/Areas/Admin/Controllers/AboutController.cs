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
    public class AboutController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<About> _sectionRepo;
        private readonly IRepository<AboutGallery> _repo;
        private readonly IRepository<Testimonial> _testimonialRepo;
        public AboutController(IMapper mapper,
            IFileService fileService,
            IRepository<About> sectionRepo,
            IRepository<AboutGallery> repo,
            IRepository<Testimonial> testimonialRepo)
        {
            _mapper = mapper;
            _fileService = fileService;
            _repo = repo;
            _sectionRepo = sectionRepo;
            _testimonialRepo = testimonialRepo;
        }
        #region Section 
        [HttpGet("/admin/about")]
        public async Task<IActionResult> GetSection()
        {
            return View(new AboutViewModel
            {
                Section = _mapper.Map<AboutDto>(await _sectionRepo.First()),
                AboutGallerys = _mapper.Map<List<AboutGalleryDto>>(await _repo.GetAll()),
                Testimonials = _mapper.Map<List<TestimonialDto>>(await _testimonialRepo.GetAll()),
            });
        }
        [HttpPost("/admin/about")]
        public async Task<IActionResult> PostSection(AboutDto model)
        {
            var entity = await _sectionRepo.First();
            await _fileService.SaveAllFiles(entity, model, "Uploads/AboutGallery");
            _mapper.Map(model, entity);
            await _sectionRepo.Update(entity);
            await _sectionRepo.SaveChanges();
            return RedirectToAction(nameof(GetSection));
        }
        #endregion

        #region aboutgallery 
        [HttpGet("/admin/about/gallery/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            return View(id > 0 ?
                _mapper.Map<AboutGalleryDto>(await _repo.Get(id))
                : new AboutGalleryDto
                {
                    IsActive = true,
                    Id = 0
                });
        }
        [HttpPost("/admin/about/gallery")]
        public async Task<IActionResult> Post(AboutGalleryDto model) 
        { 
            AboutGallery entity; 
            if (model.Id <= 0) 
            { 
                entity = _mapper.Map<AboutGallery>(model); 
                await _fileService.SaveAllFiles(entity, model, "Uploads/AboutGallery"); 
                var entities = await _repo.GetAll(); 
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1; 
                await _repo.Add(entity); 
            } 
            else 
            { 
                entity = (await _repo.Get(model.Id))!; 
                await _fileService.SaveAllFiles(entity, model, "Uploads/AboutGallery"); 
                _mapper.Map(model, entity); 
                await _repo.Update(entity); 
            } 
            await _repo.SaveChanges(); 
            return RedirectToAction(nameof(Get), new { id = entity.Id }); 
        }
        [HttpPost("/admin/about/gallery/delete/{id:long}")]
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

        #region Testimonial 
        [HttpGet("/admin/about/testimonial/{id:long}")]
        public async Task<IActionResult> GetTestimonial(long id)
        {
            return View(id > 0 ?
                _mapper.Map<TestimonialDto>(await _testimonialRepo.Get(id))
                : new TestimonialDto
                {
                    IsActive = true,
                    Id = 0
                });
        }
        [HttpPost("/admin/about/testimonial")]
        public async Task<IActionResult> PostTestimonial(TestimonialDto model)
        {
            Testimonial entity;
            if (model.Id <= 0)
            {
                entity = _mapper.Map<Testimonial>(model);
                await _fileService.SaveAllFiles(entity, model, "Uploads/Testimonial");
                var entities = await _testimonialRepo.GetAll();
                entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
                await _testimonialRepo.Add(entity);
            }
            else
            {
                entity = (await _testimonialRepo.Get(model.Id))!;
                await _fileService.SaveAllFiles(entity, model, "Uploads/Testimonial");
                _mapper.Map(model, entity);
                await _testimonialRepo.Update(entity);
            }
            await _testimonialRepo.SaveChanges();
            return RedirectToAction(nameof(GetTestimonial), new { id = entity.Id });
        }
        [HttpPost("/admin/about/testimonial/delete/{id:long}")]
        public async Task<IActionResult> DeleteTestimonial(long id)
        {
            var entity = await _testimonialRepo.Get(id);
            if (entity != null)
            {
                await _fileService.DeleteAllFiles(entity);
                await _testimonialRepo.SoftDelete(entity);
                await _testimonialRepo.SaveChanges();
            }
            return RedirectToAction(nameof(GetSection));
        }
        #endregion
    }
}
