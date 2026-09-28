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
	public class InsuranceController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IFileService _fileService;
		private readonly IRepository<InsuranceSection> _sectionRepo;
		private readonly IRepository<Insurance> _repo;
		public InsuranceController(IMapper mapper,
			IFileService fileService,
			IRepository<InsuranceSection> sectionRepo,
			IRepository<Insurance> repo)
		{
			_mapper = mapper;
			_fileService = fileService;
			_repo = repo;
			_sectionRepo = sectionRepo;
		}

		#region Section 
		[HttpGet("/admin/insurance-section")]
		public async Task<IActionResult> GetSection()
		{
			return View(new InsuranceViewModel
			{
				Section = _mapper.Map<InsuranceSectionDto>(await _sectionRepo.First()),
				Insurances = _mapper.Map<List<InsuranceDto>>(await _repo.GetAll())
			});
		}
		[HttpPost("/admin/insurance/section")]
		public async Task<IActionResult> PostSection(InsuranceSectionDto model)
		{
			var entity = await _sectionRepo.First();
			await _fileService.SaveAllFiles(entity, model, "Uploads/Insurance");
			_mapper.Map(model, entity);
			await _sectionRepo.Update(entity);
			await _sectionRepo.SaveChanges();
			return RedirectToAction(nameof(GetSection));
		}
		#endregion

		#region insurance
		[HttpGet("/admin/insurance/{id:long}")]
		public async Task<IActionResult> Get(long id)
		{
			return View(id > 0 ?
				_mapper.Map<InsuranceDto>(await _repo.Get(id))
				: new InsuranceDto
				{
					IsActive = true,
					Id = 0
				});
		}
		[HttpPost("/admin/insurance")]
		public async Task<IActionResult> Post(InsuranceDto model)
		{
			Insurance entity;
			if (model.Id <= 0)
			{
				entity = _mapper.Map<Insurance>(model);
				await _fileService.SaveAllFiles(entity, model, "Uploads/Insurance");
				var entities = await _repo.GetAll();
				entity.DisplayOrder = entities.Any() ? entities.Max(x => x.DisplayOrder) + 1 : 1;
				await _repo.Add(entity);
			}
			else
			{
				entity = (await _repo.Get(model.Id))!;
				await _fileService.SaveAllFiles(entity, model, "Uploads/Insurance");
				_mapper.Map(model, entity);
				await _repo.Update(entity);
			}
			await _repo.SaveChanges();
			return RedirectToAction(nameof(Get), new { id = entity.Id });
		}
		[HttpPost("/admin/insurance/delete/{id:long}")]
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
