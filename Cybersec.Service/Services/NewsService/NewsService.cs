using AutoMapper;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;
using Cybersec.Service.Exceptions;
using Cybersec.Service.Helpers;
using Cybersec.Service.Interfaces.News;
using Cybersec.Service.ViewModels.News;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Service.Services.NewsService;
public class NewsService : INewsService
{
    private readonly INewsRepository _newsrepository;
    private readonly IMapper _mapper;

    public NewsService(INewsRepository newsrepository, IMapper mapper)
    {
        _newsrepository = newsrepository;
        _mapper = mapper;
    }

    public async Task<NewsModel> CreateAsync(NewsPostModel dto)
    {
        var mapped = _mapper.Map<News>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.ImageUrl= await MediaHelper.UploadFile(dto.ImageUrl);

        await _newsrepository.InsertAsync(mapped);

        return _mapper.Map<NewsModel>(mapped);
    }

    public async Task<NewsModel> ModifyAsync(int id, NewsPutModel dto)
    {
        var entry = await _newsrepository.SelectAll()
            .Where(n => n.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (entry is null)
            throw new CyberException(404, "News is not found");

        var mapped= await _newsrepository.UpdateAsync(entry);
        mapped.UpdatedAt = DateTime.UtcNow;
        mapped.ImageUrl= await MediaHelper.UploadFile(dto.ImageUrl);

        await _newsrepository.UpdateAsync(mapped);

        return _mapper.Map<NewsModel>(mapped);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var entry = await _newsrepository.SelectAll()
           .Where(n => n.Id == id)
           .AsNoTracking()
           .FirstOrDefaultAsync();

        if (entry is null)
            throw new CyberException(404, "News is not found");

        await _newsrepository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<NewsModel>> RetrieveAllAsync()
    {
        var entries = await _newsrepository.SelectAll()
            .ToListAsync();

        return _mapper.Map<IEnumerable<NewsModel>>(entries);
    }

    public async Task<NewsModel> RetrieveByIdAsync(int id)
    {
        var entry = await _newsrepository.SelectAll()
          .Where(n => n.Id == id)
          .AsNoTracking()
          .FirstOrDefaultAsync();

        if (entry is null)
            throw new CyberException(404, "News is not found");

        return _mapper.Map<NewsModel>(entry);
    }
}
