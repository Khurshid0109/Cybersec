using AutoMapper;
using Cybersec.Domain.Entities;
using Cybersec.Service.Exceptions;
using Cybersec.Data.IRepositories;
using Microsoft.EntityFrameworkCore;
using Cybersec.Service.Interfaces.Users;
using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Service.Services.Users;
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IMapper mapper, IUserRepository userRepository)
    {

        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<UserModel> CreateAsync(UserPostModel dto)
    {
        var user = await _userRepository.SelectAll()
              .Where(u => u.Email.ToLower() == dto.Email.ToLower())
              .AsNoTracking()
              .FirstOrDefaultAsync();

        if (user is not null)
            throw new CyberException(409, "User already exists");

        var mapped = _mapper.Map<User>(dto);
        mapped.CreatedAt = DateTime.UtcNow;

        var result = await _userRepository.InsertAsync(mapped);

        return _mapper.Map<UserModel>(result);
    }

    public async Task<UserModel> ModifyAsync(int id, UserPutModel dto)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found!");

        var mapped = _mapper.Map(dto, user);
        mapped.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(mapped);

        return _mapper.Map<UserModel>(mapped);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var user = await _userRepository.SelectAll()
              .Where(u => u.Id.Equals(id.ToString()))
              .AsNoTracking()
              .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found!");

        await _userRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<UserModel>> RetrieveAllAsync()
    {
        var users = await _userRepository.SelectAll()
             .AsNoTracking()
             .ToListAsync();

        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<UserModel> RetrieveByEmailAsync(string email)
    {
        var user = await _userRepository.SelectAll()
                .Where(u => u.Email.ToLower() == email.ToLower())
                .AsNoTracking()
                .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found!");

        return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel> RetrieveByIdAsync(int id)
    {
        var user = await _userRepository.SelectAll()
               .Where(u => u.Id.Equals(id.ToString()))
               .AsNoTracking()
               .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found!");

        return _mapper.Map<UserModel>(user);
    }
}
