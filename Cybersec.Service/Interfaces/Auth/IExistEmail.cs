using Cybersec.Domain.Entities;
using Cybersec.Domain.Enums;
using Cybersec.Service.DTOs.Auth;

namespace Cybersec.Service.Interfaces.Auth;
public interface IExistEmail
{
    Task<EmailExistanceEnum> EmailExistance(EmailModel email);

    Task SendMessage(Message message);

    Task<bool> VerifyCodeAsync(string email, long code);

    Task<bool> ResendCodeAsync(string email);
}
