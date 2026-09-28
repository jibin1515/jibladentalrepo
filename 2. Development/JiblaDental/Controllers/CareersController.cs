using Application.Constants;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Infrastructure.Email;
using Application.Interfaces.Persistence;
using Application.Models;
using Application.Models.Framework;
using AutoMapper;
using Domain;
using Domain.Framework;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Newtonsoft.Json;
using Persistence.Services;
using Razor.Templating.Core;
using System.Net;

namespace JiblaDental.Controllers
{
	public class CareersController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Career> _repo;
		private readonly IRepository<PageSettings> _pageRepo;
		private readonly IRepository<Email> _emailRepo;
		private readonly IEmailService _emailService;
		private readonly IRepository<Enquiry> _enquiryRepo;
		private readonly IRepository<Contact> _contactRepo;
		private readonly IRepository<CareerSection> _careerSectionRepo;
        private readonly IFileService _fileService;
		private readonly IRepository<CareerContent> _careerContentRepo;

        public CareersController(IMapper mapper,
			IRepository<Career> repo,
			IRepository<PageSettings> pageRepo,
			IRepository<Email> emailRepo,
			IEmailService emailService,
			IRepository<Enquiry> enquiryRepo,
			IRepository<Contact> contactRepo,
			IRepository<CareerSection> careerSectionRepo,
            IFileService fileService,
            IRepository<CareerContent> careerContentRepo)
		{
			_mapper = mapper;
			_repo = repo;
			_pageRepo = pageRepo;
			_emailRepo = emailRepo;
			_emailService = emailService;
			_enquiryRepo = enquiryRepo;
			_contactRepo = contactRepo;
			_careerSectionRepo = careerSectionRepo;
			_fileService = fileService;
			_careerContentRepo = careerContentRepo;
		}

		[HttpGet("/careers/{*url}")]
		public async Task<IActionResult> Index(string url)
		{
			var data = _mapper.Map<CareerDto>(await _repo.FirstOrDefaultActive(x =>
			string.Equals(x.PageName, url, StringComparison.InvariantCultureIgnoreCase)));
			if (data == null)
			{
				return View(
				new HomeViewModel
				{
					Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
					Careers = _mapper.Map<List<CareerDto>>(await _repo.GetAllActive()),
					IsDetails = false,
					Enquiry = new EnquiryDto(),
					CareerSection = _mapper.Map<CareerSectionDto>(await _careerSectionRepo.First()),
                });
			}
			if (TempData["ValidationErrors"] is string)
				ModelState.AddModelError("ValidationErrors", TempData["ValidationErrors"] as string ?? string.Empty);
			return View(
					new HomeViewModel
					{
						Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
						Career = data,
						IsDetails = true,
						Enquiry = new EnquiryDto(),
                        CareerSection = _mapper.Map<CareerSectionDto>(await _careerSectionRepo.First()),
                        CareerContents = _mapper.Map<List<CareerContentDto>>(await _careerContentRepo.WhereActive(x => x.CareerId == data.Id)),
                    });
		}

		[HttpPost("/career")]
		public async Task<IActionResult> PostEnquiry(EnquiryDto model)
		{
			HomeViewModel response = ValidateCaptcha(Request.Form["g-recaptcha-response"]);
			var career = _mapper.Map<CareerDto>(await _repo.Get((long)model.EntityId));
			if (response.Success)
			{
				model.Email = model.Email!.ToLower();
				model.Purpose = EnquiryTypes.CareerApplication;            
            var entity = _mapper.Map<Enquiry>(model);
            await HandleFileUploads(entity, model);
            await _enquiryRepo.Add(entity);
				await _enquiryRepo.SaveChanges();
				var email = _mapper.Map<EmailDto>(await _emailRepo.FirstOrDefault(x => x.Purpose == EnquiryTypes.CareerApplication));
				if (!string.IsNullOrWhiteSpace(email.EmailId))
				{
					model.CreatedOn = DateTime.Now;
					var emailModel = new HomeViewModel
					{
						Contact = _mapper.Map<ContactDto>(await _contactRepo.First()),
						Enquiry = model,
						SenderEmail = email.EmailId,
						Career = career,
                    };

					if (!string.IsNullOrWhiteSpace(email.Recipients))
					{
						var enquiryHtml =
							await RazorTemplateEngine.RenderAsync("/Templates/Mail/CareerEnquiry/Enquiry.cshtml",
								emailModel);
						await _emailService.Send(new Message
						{
							From = email,
							To = email.Recipients?.Split(",").Where(x => !string.IsNullOrWhiteSpace(x))
								.Select(x => new MailboxAddress("Jibla Dental Center", x)).ToList(),
							Subject = "New enquiry from " + model.Name,
							Content = enquiryHtml,
                            AttachmentPaths = new List<string?> { model.AttachedFilePath }
                        });
					}

					//Confirmation email
					//if (!string.IsNullOrWhiteSpace(model.Email))
					//{
					//	var enquiryConfirmationHtml =
					//		await RazorTemplateEngine.RenderAsync("/Templates/Mail/CareerEnquiry/EnquiryConfirmation.cshtml",
					//			emailModel);
					//	await _emailService.Send(new Message
					//	{
					//		From = email,
					//		To = new List<MailboxAddress> { new(model.Name, model.Email) },
					//		Subject = "Thank you for contacting Jibla Dental Center",
					//		Content = enquiryConfirmationHtml
					//	});
					//}
				}

				return RedirectToAction("Result", "Home");
			}
			else
			{
				TempData["ValidationErrors"] = response.ErrorMessage[0].ToString();
				return RedirectToAction(nameof(Index), new { @url = career.PageName });
			}
		}
        private async Task HandleFileUploads(Enquiry entity, EnquiryDto model,
        CancellationToken cancellationToken = default)
        {
            if ((model.AttachedFile != null || model.AttachedFilePath == null) && entity.AttachedFilePath != null)
            {
                await _fileService.DeleteFile(entity.AttachedFilePath);
                entity.AttachedFilePath = null;
            }

            if (model.AttachedFile != null)
                entity.AttachedFilePath = model.AttachedFilePath =
                    await _fileService.SaveFile(model.AttachedFile, "Uploads/Careers/Applications/" + entity.Id,
                        cancellationToken);
        }
        public static HomeViewModel ValidateCaptcha(string response)
		{
			string secret = "6Le7qz8rAAAAAJVQqG53ontF9uNVBgiR9gieox0w";
			var client = new WebClient();
			var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
			return JsonConvert.DeserializeObject<HomeViewModel>(jsonResult.ToString());
		}

	}
}
