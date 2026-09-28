using Application.Constants;
using Domain;
using Domain.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Services;

public static class DbInitializer
{
    public static async Task Seed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Email.FirstOrDefault(x => x.Purpose == EnquiryTypes.General) == null)
            await context.AddAsync(new Email { Purpose = EnquiryTypes.General });

        if (context.Email.FirstOrDefault(x => x.Purpose == EnquiryTypes.CareerApplication) == null)
            await context.AddAsync(new Email { Purpose = EnquiryTypes.CareerApplication });

        if (context.Email.FirstOrDefault(x => x.Purpose == EnquiryTypes.Newsletter) == null)
            await context.AddAsync(new Email { Purpose = EnquiryTypes.Newsletter });

        if (!context.About.Any())
            context.About.Add(new About());

        if (!context.Contact.Any())
            context.Contact.Add(new Contact());

        if (!context.InsuranceSection.Any())
            context.InsuranceSection.Add(new InsuranceSection());

        if (!context.PartnerSection.Any())
            context.PartnerSection.Add(new PartnerSection());

        if (!context.ServiceSection.Any())
            context.ServiceSection.Add(new ServiceSection());

        if (!context.TeamSection.Any())
            context.TeamSection.Add(new TeamSection());

		if (!context.CareerSection.Any())
			context.CareerSection.Add(new CareerSection());

		await context.SaveChangesAsync();
    }
}