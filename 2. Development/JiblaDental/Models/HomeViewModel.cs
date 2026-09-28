using Application.Models;
using Newtonsoft.Json;

namespace JiblaDental.Models
{
    public class HomeViewModel
    {
        public List<PageSettingsDto>? Pages { get; set; }
        public EnquiryDto? Enquiry { get; set; }
        public List<SocialMediaDto>? SocialMedia { get; set; }
        public bool IsDetails { get; set; }
        public ContactDto? Contact { get; set; }
        public List<HomeBannerDto>? HomeBanner { get; set; }
        public AboutDto? About { get; set; }
        public ServiceSectionDto? ServiceSection { get; set; }
        public List<ServiceDto>? Services { get; set; }
        public ServiceDto? Service { get; set; }
        public TeamSectionDto? TeamSection { get; set; }
        public List<TeamDto>? Teams { get; set; }
        public TeamDto? Team { get; set; }
        public List<DepartmentDto>? Departments { get; set; }
        public DepartmentDto? Department { get; set; }
        public List<TeamContentDto>? TeamContent { get; set; }
        public PartnerSectionDto? PartnerSection { get; set; }
        public List<PartnerDto>? Partners { get; set; }
        public PartnerDto? Partner { get; set; }
        public List<PartnerCategoryDto>? PartnerCategories { get; set; }
        public List<AboutGalleryDto>? AboutGalleries { get; set; }
        public List<TestimonialDto>? Testimonials { get; set; }
        public List<LoyaltyDto>? Loyalties { get; set; }
        public LoyaltyDto? Loyalty { get; set; }
        public List<LoyaltyImageDto>? LoyaltyImages { get; set; }
        public List<PolicyDto>? Policy { get; set; }
        public List<FaqDto>? Faq { get; set; }
        public List<NewsDto>? NewsList { get; set; }
        public NewsDto? News { get; set; }
        public List<NewsImageDto>? NewsImages { get; set; }
        public List<InsuranceDto>? Insurance { get; set; }
        public InsuranceSectionDto? InsuranceSection { get; set; }
        public List<VideoHubDto>? VideoHubs { get; set; }
        public List<CareerDto>? Careers { get; set; }
        public CareerDto? Career { get; set; }
        public CareerSectionDto? CareerSection { get; set; }
        public List<CareerContentDto>? CareerContents { get; set; }


		public string? SenderEmail { get; set; }
        public string? Message { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
        [JsonProperty("error-codes")] public List<string>? ErrorMessage { get; set; }
    }
}
