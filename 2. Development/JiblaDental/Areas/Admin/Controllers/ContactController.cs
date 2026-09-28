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
    public class ContactController : Controller
    {
        private readonly IMapper _mapper; 
        private readonly IFileService _fileService; 
        private readonly IRepository<Contact> _repo; 
        public ContactController(IMapper mapper, 
            IFileService fileService, 
            IRepository<Contact> repo) 
        { 
            _mapper = mapper; 
            _fileService = fileService; 
            _repo = repo; 
        }
        
        #region contact 
        [HttpGet("/admin/contact")]
        public async Task<IActionResult> Get() 
        { 
            return View(_mapper.Map<ContactDto>(await _repo.First())); 
        }
        [HttpPost("/admin/contact")]
        public async Task<IActionResult> Post(ContactDto model) 
        { 
            var entity = await _repo.First(); 
            _mapper.Map(model, entity); 
            await _fileService.SaveAllFiles(entity, model, "Uploads/Contact"); 
            await _repo.Update(entity); 
            await _repo.SaveChanges(); 
            return RedirectToAction(nameof(Get)); 
        } 
        #endregion
    }
}
