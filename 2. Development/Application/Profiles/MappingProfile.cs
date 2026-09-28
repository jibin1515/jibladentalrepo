using Application.Models;
using Application.Models.Common;
using Application.Models.Framework;
using AutoMapper;
using Domain;
using Domain.Common;
using Domain.Framework;

namespace Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BaseEntity, BaseDto>()
            .IncludeAllDerived()
            .ReverseMap()
            .ForPath(x => x.Id, x => x.Ignore())
            .ForPath(x => x.CreatedOn, x => x.Ignore())
            .ForPath(x => x.ModifiedOn, x => x.Ignore())
            .ForPath(x => x.ModifiedOn, x => x.Ignore())
            .ForPath(x => x.ModifiedBy, x => x.Ignore())
            .ForPath(x => x.IsDeleted, x => x.Ignore());

        CreateMap<OrderableBaseEntity, OrderableDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);

        //Page
        CreateMap<PageSettings, PageSettingsDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //SocialMedia
        CreateMap<SocialMedia, SocialMediaDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Enquiry
        CreateMap<Enquiry, EnquiryDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Email
        CreateMap<Email, EmailDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Contact
        CreateMap<Contact, ContactDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //VideoHub
        CreateMap<VideoHub, VideoHubDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

		//Insurance
		CreateMap<InsuranceSection, InsuranceSectionDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap(); 
        CreateMap<Insurance, InsuranceDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //News
        CreateMap<News, NewsDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap(); 
        CreateMap<NewsImage, NewsImageDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Faq
        CreateMap<Faq, FaqDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Faq
        CreateMap<Policy, PolicyDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Loyalty
        CreateMap<Loyalty, LoyaltyDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
        CreateMap<LoyaltyImage, LoyaltyImageDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Partner
        CreateMap<PartnerSection, PartnerSectionDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
        CreateMap<Partner, PartnerDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
        CreateMap<PartnerCategory, PartnerCategoryDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Service
        CreateMap<ServiceSection, ServiceSectionDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
        CreateMap<Service, ServiceDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //About
        CreateMap<About, AboutDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap(); 
        CreateMap<AboutGallery, AboutGalleryDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
        CreateMap<Testimonial, TestimonialDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Team
        CreateMap<TeamSection, TeamSectionDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap(); 
        CreateMap<Department, DepartmentDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap(); 
        CreateMap<Team, TeamDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
        CreateMap<TeamContent, TeamContentDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //HomeBanner
        CreateMap<HomeBanner, HomeBannerDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();

        //Career
        CreateMap<Career, CareerDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
		CreateMap<CareerSection, CareerSectionDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
		CreateMap<CareerContent, CareerContentDto>().IncludeBase<BaseEntity, BaseDto>().ReverseMap();
	}
}