using AutoMapper;
using Cybersec.Domain.Entities;
using Cybersec.Service.Exceptions;
using Cybersec.Data.IRepositories;
using Microsoft.EntityFrameworkCore;
using Cybersec.Service.Interfaces.Users;
using Cybersec.Service.ViewModels.Code;

namespace Cybersec.Service.Services.Users;
public class UserCodeService:IUserCodeService
{
    private readonly IMapper _mapper;
    private readonly IUserCodeRepository _repository;
    private readonly IUserRepository _userRepository;

    public UserCodeService(IMapper mapper, 
                           IUserCodeRepository repository, 
                           IUserRepository userRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<UserCodeViewModel> CreateAsync(UserCodePostModel dto)
    {
        var user = await _userRepository.SelectAll()
             .Where(u => u.Id == dto.UserId)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found");

        var mapped = _mapper.Map<UserCode>(dto);
        mapped.CreatedAt = DateTime.UtcNow;

        var result = await _repository.InsertAsync(mapped);

        return _mapper.Map<UserCodeViewModel>(result);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var code = await _repository.SelectAll()
             .Where(c => c.Id == id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (code is null)
            throw new CyberException(404, "Code is not found!");

        await _repository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<UserCodeViewModel>> RetrieveAllAsync()
    {
       var codes = await _repository.SelectAll()
            .ToListAsync();

        return _mapper.Map<IEnumerable<UserCodeViewModel>>(codes);  
    }

    public async Task<UserCodeViewModel> RetrieveByIdAsync(int id)
    {
        var code = await _repository.SelectAll()
             .Where(c => c.Id == id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (code is null)
            throw new CyberException(404, "Code is not found!");

        return _mapper.Map<UserCodeViewModel>(code);
    }
}
