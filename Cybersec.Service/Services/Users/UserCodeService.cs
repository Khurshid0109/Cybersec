using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;
using Cybersec.Domain.Enums;
using Cybersec.Service.DTOs.Code;
using Cybersec.Service.Exceptions;
using Cybersec.Service.Extentions;
using Cybersec.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Service.Services.Users;
public class UserCodeService(
    IRepository<UserCode> userCodeRepository,
    IMapper mapper,
    IRepository<User> userRepository)
    : IUserCodeService
{
    public async Task<UserCodeViewModel> CreateAsync(UserCode model)
    {
        var user = await userRepository.SelectAll()
            .IgnoreQueryFilters()
            .Where(u => u.Id == model.UserId)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found!");

        model.ExpireDate = DateTime.UtcNow.AddMinutes(3);
        model.CreatedAt = DateTime.UtcNow;

        var result = await userCodeRepository.InsertAsync(model);

        return mapper.Map<UserCodeViewModel>(result);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var code = await userCodeRepository.SelectAll()
             .Where(c => c.Id == id)
             .FirstOrDefaultAsync();

        if (code is null)
            throw new CyberException(404, "Code is not found!");

        await userCodeRepository.DeleteAsync(id);

        return true;
    }

    public async Task<PaginationViewModel<UserCodeViewModel>> GetAllAsync(PaginationParams @params, bool deleted = false)
    {
        var codes = await userCodeRepository.SelectAll()
             .IgnoreQueryFilters()
             .Where(uc => deleted ? uc.Status == Status.Deleted:uc.Status == Status.Active)
             .AsNoTracking()
             .ProjectTo<UserCodeViewModel>(mapper.ConfigurationProvider)
             .ToPaginationAsync(@params);

        return codes;
    }

    public async Task<UserCodeViewModel> GetByIdAsync(long id)
    {
        var code = await userCodeRepository.SelectAll()
             .Where(c => c.Id == id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (code is null)
            throw new CyberException(404, "Code is not found!");

        return mapper.Map<UserCodeViewModel>(code);
    }
}
