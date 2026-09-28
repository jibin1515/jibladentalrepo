using Application.Constants;
using Application.Interfaces.Persistence;
using Application.Models;
using Application.Models.Framework;
using AutoMapper;
using JiblaDental.Areas.Admin.Models;
using Domain;
using Domain.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class EnquiryController : Controller
{
    private readonly IRepository<Email> _emailRepo;
    private readonly IRepository<Enquiry> _enquiryRepo;
    private readonly IMapper _mapper;
    private readonly IRepository<Career> _careerRepo;

    public EnquiryController(
        IRepository<Email> emailRepo,
        IRepository<Enquiry> enquiryRepo,
        IMapper mapper,
        IRepository<Career> careerRepo)
    {
        _emailRepo = emailRepo;
        _enquiryRepo = enquiryRepo;
        _mapper = mapper;
        _careerRepo = careerRepo;
    }

    #region Email

    [HttpGet("/admin/enquiry")]
    public async Task<IActionResult> GetAll(string type = EnquiryTypes.General)
    {
        return View(new EnquiryViewModel
        {
            Email = _mapper.Map<EmailDto>(await _emailRepo.First(x => x.Purpose == type)),
            Enquiries = _mapper.Map<List<EnquiryDto>>(await _enquiryRepo.Where(x => x.Purpose == type))
                .OrderByDescending(x => x.CreatedOn)
                .ToList(),
            Careers = _mapper.Map<List<CareerDto>>(await _careerRepo.GetAll())
        });
    }

    [HttpPost("/admin/email")]
    public async Task<IActionResult> PostEmail(EmailDto model)
    {
        var entity = await _emailRepo.First(x => x.Purpose == model.Purpose);

        _mapper.Map(model, entity);

        await _emailRepo.Update(entity);
        await _emailRepo.SaveChanges();


        return RedirectToAction(nameof(GetAll), new { type = model.Purpose });
    }

    #endregion

    #region Enquiry

    [HttpGet("/admin/enquiry/{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var enquiry = _mapper.Map<EnquiryDto>(await _enquiryRepo.Get(id));
        return View(new EnquiryViewModel
        {
            Enquiry = enquiry,
            Career = (enquiry.EntityId != null && enquiry.EntityId != 0) ? _mapper.Map<CareerDto>(await _careerRepo.Get(enquiry.EntityId.Value)) : new CareerDto(),
        });
    }

    [HttpPost("/admin/enquiry/delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var entity = await _enquiryRepo.Get(id);
        if (entity != null)
        {
            await _enquiryRepo.SoftDelete(entity);
            await _enquiryRepo.SaveChanges();
        }

        return RedirectToAction(nameof(GetAll), new { type = entity.Purpose });
    }

    #endregion
}