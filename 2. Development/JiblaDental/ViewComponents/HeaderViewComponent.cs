using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly IMapper _mapper;
    private readonly IRepository<PageSettings> _pageRepo;
    private readonly IRepository<SocialMedia> _socialMediaRepo;
    private readonly IRepository<Contact> _contactRepo;

    public HeaderViewComponent(
        IMapper mapper,
        IRepository<PageSettings> pageRepo,
        IRepository<SocialMedia> socialMediaRepo,
        IRepository<Contact> contactRepo)
    {
        _mapper = mapper;
        _pageRepo = pageRepo;
        _socialMediaRepo = socialMediaRepo;
        _contactRepo = contactRepo;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("Default", new HeaderViewModel
        {
            Pages = _mapper.Map<List<PageSettingsDto>>(await _pageRepo.GetAllActive()),
            SocialMedia = _mapper.Map<List<SocialMediaDto>>(await _socialMediaRepo.GetAllActive()),
            Contact = _mapper.Map<ContactDto>(await _contactRepo.First())
		});
    }
}