using System.Net;
using AutoMapper;
using System.Text;
using System.Net.Mail;
using Cybersec.Domain.Enums;
using System.Security.Claims;
using Cybersec.Domain.Entities;
using Cybersec.Service.Exceptions;
using Cybersec.Data.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.ViewModels.Users;
using Cybersec.Service.ViewModels.Codes;
using Cybersec.Service.Interfaces.Users;
using Microsoft.Extensions.Configuration;

namespace Cybersec.Service.Services.Auth;
public class UserAuthentication : IUserAuthentication
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IUserCodeService _userCodeService;
    private readonly IMapper _mapper;

    public UserAuthentication(IUserRepository userRepository,
        IMapper mapper,
        IConfiguration configuration,
        IUserCodeService userCodeService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
        _userCodeService = userCodeService;
    }

    public async Task<EmailExistance> CheckEmail(string email)
    {
        var user = await _userRepository.SelectAll()
             .Where(u => u.Email.ToLower() == email.ToLower())
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            return EmailExistance.NotFound;

        if( user is not null && user.isVerified == false)
        {
            //Implement sent code here

            return EmailExistance.NotVerified;
        }

        return EmailExistance.Found;
    }

 

    public Task<string> Login(LoginViewModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Register(UserPostModel model)
    {
        var user = await _userRepository.SelectAll()
              .Where(u => u.Email.ToLower() == model.Email.ToLower())
              .AsNoTracking()
              .FirstOrDefaultAsync();

        if (user is not null)
            throw new CyberException(409, "User already exists");

        var mapped = _mapper.Map<User>(model);
        mapped.CreatedAt = DateTime.UtcNow;

        var result = await _userRepository.InsertAsync(mapped);
        await GenerateCode(result);
        var token =  GenerateToken(result);
        return token;
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                 new Claim("Id", user.Id.ToString()),
                 new Claim(ClaimTypes.Role,user.Role.ToString()),
                 new Claim(ClaimTypes.Email, user.Email),
            }),
            Audience = _configuration["JWT:Audience"],
            Issuer = _configuration["JWT:Issuer"],
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:Expire"])),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Task SendMessage(Message message)
    {
        var _smtpModel = new
        {
            Host = _configuration["Host"],
            Email = (string)_configuration["EmailAddress"] ?? throw new ArgumentNullException(),
            Port = 587,
            AppPassword = _configuration["Password"]
        };

        using (MailMessage mm = new MailMessage(_smtpModel.Email, message.To))
        {
            mm.Subject = message.Subject;
            mm.Body = message.Body;
            mm.IsBodyHtml = false;
            using (System.Net.Mail.SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = _smtpModel.Host;
                smtp.Port = _smtpModel.Port;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                NetworkCredential NetworkCred = new NetworkCredential(_smtpModel.Email, _smtpModel.AppPassword);
                smtp.Credentials = NetworkCred;
                smtp.Send(mm);
            }
        }

        return Task.CompletedTask;
    }

    public async Task<bool> GenerateCode(User user)
    {
        var randomNumber = new Random().Next(100000, 999999);

        var message = new Message()
        {
            Subject = "Do not give this code to Others",
            To = user.Email,
            Body = $"{randomNumber}"
        };

        var newCode = new UserCodePostModel
        {
            UserId = user.Id,
            Code = randomNumber
        };

        await _userCodeService.CreateAsync(newCode);
        await SendMessage(message);

        return true;
    }
}
