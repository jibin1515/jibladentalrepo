using Application.Constants;
using Application.Interfaces.Infrastructure.Email;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Persistence;
using Application.Models.Framework;
using Application.Models;
using AutoMapper;
using Domain.Framework;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Newtonsoft.Json;
using System.Net;
using Razor.Templating.Core;

namespace JiblaDental.Controllers
{
	public class ContactController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRepository<PageSettings> _pageSettingsRepo;
		private readonly IRepository<Email> _emailRepo;
		private readonly IEmailService _emailService;
		private readonly IRepository<Enquiry> _enquiryRepo;
		private readonly IRepository<Contact> _contactRepo;

		public ContactController(IMapper mapper,
			IRepository<PageSettings> pageSettingsRepo,
			IRepository<Email> emailRepo,
			IEmailService emailService,
			IRepository<Enquiry> enquiryRepo,
			IRepository<Contact> contactRepo)
		{
			_mapper = mapper;
			_pageSettingsRepo = pageSettingsRepo;
			_emailRepo = emailRepo;
			_emailService = emailService;
			_enquiryRepo = enquiryRepo;
			_contactRepo = contactRepo;
		}

		[HttpGet("/contact")]
		public async Task<IActionResult> Index()
		{
			if (TempData["ValidationErrors"] is string)
				ModelState.AddModelError("ValidationErrors", TempData["ValidationErrors"] as string ?? string.Empty);
			return View(
			 new HomeViewModel
			 {
				 Pages = _mapper.Map<List<PageSettingsDto>>(await _pageSettingsRepo.GetAllActive()),
				 Contact = _mapper.Map<ContactDto>(await _contactRepo.First()),
				 Enquiry = new EnquiryDto(),
			 });
		}

		//[ValidateReCaptcha]
		[HttpPost("/contact")]
		public async Task<IActionResult> PostEnquiry(EnquiryDto model)
		{
			HomeViewModel response = ValidateCaptcha(Request.Form["g-recaptcha-response"]);
			if (response.Success)
			{
				model.Email = model.Email!.ToLower();
				model.Purpose = EnquiryTypes.General;
				var entity = _mapper.Map<Enquiry>(model);
				await _enquiryRepo.Add(entity);
				await _enquiryRepo.SaveChanges();
				var email = _mapper.Map<EmailDto>(await _emailRepo.FirstOrDefault(x => x.Purpose == EnquiryTypes.General));
				if (!string.IsNullOrWhiteSpace(email.EmailId))
				{
					model.CreatedOn = DateTime.Now;
					var emailModel = new HomeViewModel
					{
						Contact = _mapper.Map<ContactDto>(await _contactRepo.First()),
						Enquiry = model,
						SenderEmail = email.EmailId
					};

					if (!string.IsNullOrWhiteSpace(email.Recipients))
					{
						var enquiryHtml =
							await RazorTemplateEngine.RenderAsync("/Templates/Mail/Enquiry/AdminNotification.cshtml",
								emailModel);
						await _emailService.Send(new Message
						{
							From = email,
							To = email.Recipients?.Split(",").Where(x => !string.IsNullOrWhiteSpace(x))
								.Select(x => new MailboxAddress("Jibla Dental Center", x)).ToList(),
							Subject = "New enquiry from " + model.Name,
							Content = enquiryHtml
						});
					}

					//Confirmation email
					//if (!string.IsNullOrWhiteSpace(model.Email))
					//{
					//	var enquiryConfirmationHtml =
					//		await RazorTemplateEngine.RenderAsync("/Templates/Mail/Enquiry/SenderConfirmation.cshtml",
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
				return RedirectToAction(nameof(Index));
			}
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
