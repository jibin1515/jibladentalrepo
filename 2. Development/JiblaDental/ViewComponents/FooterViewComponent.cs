using Application.Interfaces.Persistence;
using Application.Models;
using AutoMapper;
using Domain;
using JiblaDental.Models;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly IMapper _mapper;
    private readonly IRepository<PageSettings> _pageSettingsRepo;
    private readonly IRepository<SocialMedia> _socialMediaRepo;
    private readonly IRepository<Contact> _contactRepo;

    public FooterViewComponent(
        IMapper mapper,
        IRepository<PageSettings> pageSettingsRepo,
        IRepository<SocialMedia> socialMediaRepo,
        IRepository<Contact> contactRepo)
    {
        _mapper = mapper;
        _pageSettingsRepo = pageSettingsRepo;
        _socialMediaRepo = socialMediaRepo;
        _contactRepo = contactRepo;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("Default", new FooterViewModel
        {
            Pages = _mapper.Map<List<PageSettingsDto>>(await _pageSettingsRepo.GetAllActive()),
            SocialMedia = _mapper.Map<List<SocialMediaDto>>(await _socialMediaRepo.GetAllActive()),
            Contact = _mapper.Map<ContactDto>(await _contactRepo.First())
        });
    }
}