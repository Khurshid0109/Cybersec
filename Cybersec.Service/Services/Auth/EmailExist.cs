﻿using Cybersec.Domain.Entities;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.Interfaces.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Net.Mail;
using System.Net;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Enums;
using Cybersec.Service.Exceptions;

namespace Cybersec.Service.Services.Auth;
public class EmailExist(
    IUserRepository repository,
    IConfiguration configuration,
    IUserCodeService userCodeService,
    IUserCodeRepository codeRepository)
    : IExistEmail
{
    private readonly IConfiguration _configuration = configuration.GetSection("Email");

    public async Task<EmailExistanceEnum> EmailExistance(string email)
    {
        email = email.ToLower();
        var user = await repository
            .SelectAll()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
            return EmailExistanceEnum.NotFound;

        if (user.IsVerified)
            return EmailExistanceEnum.Found;

        var resend = await ResendCodeAsync(email);

        if (!resend)
            throw new CyberException(403, "Birozdan keyinroq qayta urinib ko'ring!");

        return EmailExistanceEnum.NotVerified;
    }

    public async Task<bool> VerifyCodeAsync(string email, long code)
    {
        email = email.ToLower();
        var userCodeAny = await codeRepository
            .SelectAll()
            .IgnoreQueryFilters()
            .Include(u => u.User)
            .AnyAsync(c => c.User.Email.ToLower() == email && c.ExpireDate > DateTime.UtcNow && c.Code == code);

        if (userCodeAny)
        {
            var user = await repository
                .SelectAll()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (user is null)
                return false;

            if (!user.IsVerified)
            {
                user.IsVerified = true;
                repository.Update(user);
                await repository.SaveAsync();
            }
            return true;
        }

        return false;
    }

    public async Task<bool> ResendCodeAsync(string email)
    {
        email = email.ToLower();
        var userCodeAny = await codeRepository.SelectAll(includes: ["User"])
            .AnyAsync(c => c.User.Email.ToLower() == email && c.ExpireDate > DateTime.UtcNow);

        if (userCodeAny)
            return false;

        var user = await repository
            .SelectAll()
            .IgnoreQueryFilters()
            .Where(u => u.Email.ToLower() == email)
            .Select(u => new { u.Id })
            .FirstOrDefaultAsync();

        if (user is null)
            return false;

        var randomNumber = new Random().Next(100000, 999999);

        var message = new Message()
        {
            Subject = "TestHouse Bu kodni boshqalarga bermang!",
            To = email,
            Body = $"TestHouse platformasi uchun sizning tasdiqlash kodingiz: {randomNumber}"
        };


        var userCode = new UserCode()
        {
            Code = randomNumber,
            UserId = user.Id,
            ExpireDate = DateTime.UtcNow.AddMinutes(Constants.EMAIL_RESEND_EXPIRE_MINUTES)
        };

        _ = await userCodeService.CreateAsync(userCode);
        await this.SendMessage(message);

        return true;
    }

    public Task SendMessage(Message message)
    {
        var _smtpModel = new
        {
            Host = _configuration["Host"]!,
            Email = (string)_configuration["EmailAddress"]!,
            Port = 587,
            AppPassword = _configuration["Password"]!
        };

        using (MailMessage mm = new(_smtpModel.Email, message.To))
        {
            mm.Subject = message.Subject;
            mm.Body = message.Body;
            mm.IsBodyHtml = false;
            using SmtpClient smtp = new();
            smtp.Host = _smtpModel.Host;
            smtp.Port = _smtpModel.Port;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            NetworkCredential NetworkCred = new(_smtpModel.Email, _smtpModel.AppPassword);
            smtp.Credentials = NetworkCred;
            smtp.Send(mm);
        }

        return Task.CompletedTask;
    }

}
