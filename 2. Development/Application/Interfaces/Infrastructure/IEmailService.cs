using Application.Interfaces.Infrastructure.Email;
using Application.Models.Framework;

namespace Application.Interfaces.Infrastructure;

public interface IEmailService
{
    public Task Send(Message message);

    public Task Authenticate(EmailDto email);
}