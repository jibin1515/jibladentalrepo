using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PageSettingsConfiguration : IEntityTypeConfiguration<PageSettings>
{
	public void Configure(EntityTypeBuilder<PageSettings> builder)
	{
		builder.HasData(
			// Main Pages
			new PageSettings
			{
				Id = 1,
				Name = "index",
				Title = "{\"en\":\"Home\",\"ar\":\"AHome\"}",
				DisplayOrder = 1
			},
			new PageSettings
			{
				Id = 2,
				Name = "our-center",
				ParentName = "index",
				Title = "{\"en\":\"Our Center\",\"ar\":\"AOur Center\"}",
				DisplayOrder = 2
			},
			new PageSettings
			{
				Id = 3,
				Name = "services",
				ParentName = "index",
				Title = "{\"en\":\"Our Services\",\"ar\":\"AOur Services\"}",
				DisplayOrder = 3
			},
			new PageSettings
			{
				Id = 4,
				Name = "team",
				ParentName = "index",
				Title = "{\"en\":\"Our Team\",\"ar\":\"AOur Team\"}",
				DisplayOrder = 4
			},
			new PageSettings
			{
				Id = 5,
				Name = "partners",
				ParentName = "index",
				Title = "{\"en\":\"Our Partners\",\"ar\":\"AOur Partners\"}",
				DisplayOrder = 5
			},
			new PageSettings
			{
				Id = 6,
				Name = "loyalty-program",
				ParentName = "index",
				Title = "{\"en\":\"Loyalty Program\",\"ar\":\"ALoyalty Program\"}",
				DisplayOrder = 6
			},
			new PageSettings
			{
				Id = 7,
				Name = "contact",
				ParentName = "index",
				Title = "{\"en\":\"Contact Us\",\"ar\":\"AContact Us\"}",
				DisplayOrder = 7
			},
			new PageSettings
			{
				Id = 8,
				Name = "privacy-policy",
				ParentName = "index",
				Title = "{\"en\":\"Privacy Policy\",\"ar\":\"APrivacy Policy\"}",
				DisplayOrder = 8
			},
			new PageSettings
			{
				Id = 9,
				Name = "faq",
				ParentName = "index",
				Title = "{\"en\":\"FAQ\",\"ar\":\"AFAQ\"}",
				DisplayOrder = 9
			},
			new PageSettings
			{
				Id = 10,
				Name = "news",
				ParentName = "index",
				Title = "{\"en\":\"News\",\"ar\":\"ANews\"}",
				DisplayOrder = 10
			},
			new PageSettings
			{
				Id = 11,
				Name = "insurance",
				ParentName = "index",
				Title = "{\"en\":\"Insurance\",\"ar\":\"AInsurance\"}",
				DisplayOrder = 11
			},
			new PageSettings
			{
				Id = 12,
				Name = "video-hub",
				ParentName = "index",
				Title = "{\"en\":\"Video Hub\",\"ar\":\"AVideo Hub\"}",
				DisplayOrder = 12
			},
			new PageSettings
			{
				Id = 13,
				Name = "careers",
				ParentName = "index",
				Title = "{\"en\":\"Careers\",\"ar\":\"ACareers\"}",
				DisplayOrder = 13
			},
			new PageSettings
			{
				Id = 14,
				Name = "result",
				ParentName = "index",
				Title = "{\"en\":\"Result\",\"ar\":\"AResult\"}",
				DisplayOrder = 14
			},
			new PageSettings
			{
				Id = 15,
				Name = "error",
				ParentName = "index",
				Title = "{\"en\":\"Error\",\"ar\":\"AError\"}",
				DisplayOrder = 15
			},
			new PageSettings
			{
				Id = 16,
				Name = "appointment",
				ParentName = "about",
				Title = "{\"en\":\"Book Appointment\",\"ar\":\"ABook Appointment\"}",
				DisplayOrder = 16,
				IsDeleted = true,
				IsActive = true,
			}
		);
	}
}