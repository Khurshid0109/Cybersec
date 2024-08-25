using Cybersec.Domain.Entities;
using Cybersec.Domain.Enums;

namespace Cybersec.Service.Interfaces.Auth;
public interface IExistEmail
{
    Task<EmailExistanceEnum> EmailExistance(string email);

    Task SendMessage(Message message);

    Task<bool> VerifyCodeAsync(string email, long code);

    Task<bool> ResendCodeAsync(string email);
}
