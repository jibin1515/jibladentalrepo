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
    public async Task<IActionResult> GetAll(string type = EnquiryTypes.General, long? careerId = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var list = await _enquiryRepo.Where(x => x.Purpose == type);

        if (careerId.HasValue && careerId.Value > 0)
        {
            list = list.Where(x => x.EntityId == careerId.Value).ToList();
        }

        if (startDate.HasValue)
        {
            list = list.Where(x => x.CreatedOn >= startDate.Value.Date).ToList();
        }

        if (endDate.HasValue)
        {
            var toDate = endDate.Value.Date.AddDays(1).AddTicks(-1);
            list = list.Where(x => x.CreatedOn <= toDate).ToList();
        }

        return View(new EnquiryViewModel
        {
            Email = _mapper.Map<EmailDto>(await _emailRepo.First(x => x.Purpose == type)),
            Enquiries = _mapper.Map<List<EnquiryDto>>(list)
                .OrderByDescending(x => x.CreatedOn)
                .ToList(),
            Careers = _mapper.Map<List<CareerDto>>(await _careerRepo.GetAll()),
            SelectedCareerId = careerId,
            StartDate = startDate,
            EndDate = endDate
        });
    }

    [HttpPost("/admin/enquiry/export")]
    public async Task<IActionResult> Export([FromForm] string type, [FromForm] string? selectedIds, [FromForm] long? careerId, [FromForm] DateTime? startDate, [FromForm] DateTime? endDate)
    {
        var list = await _enquiryRepo.Where(x => x.Purpose == type);

        if (!string.IsNullOrWhiteSpace(selectedIds))
        {
            var ids = selectedIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToHashSet();
            list = list.Where(x => ids.Contains(x.Id)).ToList();
        }
        else
        {
            if (careerId.HasValue && careerId.Value > 0)
            {
                list = list.Where(x => x.EntityId == careerId.Value).ToList();
            }
            if (startDate.HasValue)
            {
                list = list.Where(x => x.CreatedOn >= startDate.Value.Date).ToList();
            }
            if (endDate.HasValue)
            {
                var toDate = endDate.Value.Date.AddDays(1).AddTicks(-1);
                list = list.Where(x => x.CreatedOn <= toDate).ToList();
            }
        }

        var careers = (await _careerRepo.GetAll()).ToDictionary(x => x.Id, x => Application.Helpers.Localization.GetEnglish(x.Title) ?? x.Title ?? "");

        var builder = new System.Text.StringBuilder();
        builder.AppendLine("ID,First Name,Last Name,Full Name,Email,Phone,Applied Position,Sent Date,Message,Attached Resume Path");

        foreach (var item in list)
        {
            var careerTitle = (item.EntityId.HasValue && careers.ContainsKey(item.EntityId.Value)) ? careers[item.EntityId.Value] : "";
            var escape = (string? val) => $"\"{val?.Replace("\"", "\"\"") ?? ""}\"";
            var sentDate = item.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
            
            builder.AppendLine($"{item.Id},{escape(item.FirstName)},{escape(item.LastName)},{escape(item.Name)},{escape(item.Email)},{escape(item.Phone)},{escape(careerTitle)},{sentDate},{escape(item.Message)},{escape(item.AttachedFilePath)}");
        }

        var bytes = System.Text.Encoding.UTF8.GetPreamble().Concat(System.Text.Encoding.UTF8.GetBytes(builder.ToString())).ToArray();
        var fileName = $"Enquiries_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

        return File(bytes, "text/csv", fileName);
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