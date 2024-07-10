using AutoMapper;
using Cybersec.Data.IRepositories;
using Cybersec.Data.Repositories;
using Cybersec.Domain.Entities;
using Cybersec.Service.Exceptions;
using Cybersec.Service.Helpers;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Service.ViewModels.Article;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Cybersec.Service.Services.Articles;
public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepo;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public ArticleService(IArticleRepository articleRepo, IMapper mapper)
    {
        _articleRepo = articleRepo;
        _mapper = mapper;
    }

    public async Task<ArticleViewModel> CreateArticleAsync(ArticlePostModel model)
    {
        var article = _mapper.Map<Article>(model);
        article.Blocks = new List<ContentBlock>();
        // Logging the input model for debugging
        Console.WriteLine("Orders: " + model.Orders);
        Console.WriteLine("Texts: " + string.Join(", ", model.Texts));
        Console.WriteLine("Images: " + model.Images.Count);
        Console.WriteLine("Videos: " + model.Videos.Count);
        Console.WriteLine("Codes: " + model.Codes.Count);

        var orderList = model.Orders.Split(',').ToList();

        Console.WriteLine("Order List: " + string.Join(", ", orderList));

        int imageIndex = 0, videoIndex = 0, textIndex = 0, codeIndex = 0;

        for (int i = 0; i < orderList.Count; i++)
        {
            if (orderList[i] == "text" && textIndex < model.Texts.Count)
            {
                article.Blocks.Add(new TextBlock { Order = i, Text = model.Texts[textIndex++] });
            }
            else if (orderList[i] == "image" && imageIndex < model.Images.Count)
            {
                var uniqueFileName = await MediaHelper.UploadFile(model.Images[imageIndex], "image");
                article.Blocks.Add(new ImageBlock { Order = i, ImageFilePath = Path.Combine("Images", uniqueFileName) });
                imageIndex++;
            }
            else if (orderList[i] == "video" && videoIndex < model.Videos.Count)
            {
                var uniqueFileName = await MediaHelper.UploadFile(model.Videos[videoIndex], "video");
                article.Blocks.Add(new VideoBlock { Order = i, VideoFilePath = Path.Combine("Videos", uniqueFileName) });
                videoIndex++;
            }
            else if (orderList[i] == "code" && codeIndex < model.Codes.Count)
            {
                article.Blocks.Add(new CodeBlock { Order = i, Code = model.Codes[codeIndex++] });
            }
        }

        article.CreatedAt = DateTime.UtcNow;

        var result = await _articleRepo.InsertAsync(article);

        return _mapper.Map<ArticleViewModel>(result);
    }

    public async Task<bool> DeleteArticleAsync(long id)
    {
        var article = await _articleRepo.SelectAll()
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (article is null)
            throw new CyberException(404, "Article is not found");

        
            foreach (var block in article.Blocks)
            {
                if (block is ImageBlock imageBlock)
                {
                    var imagePath = Path.Combine(_environment.WebRootPath, imageBlock.ImageFilePath);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                else if (block is VideoBlock videoBlock)
                {
                    var videoPath = Path.Combine(_environment.WebRootPath, videoBlock.VideoFilePath);
                    if (System.IO.File.Exists(videoPath))
                    {
                        System.IO.File.Delete(videoPath);
                    }
                }
            }
            await _articleRepo.DeleteAsync(id);
        return true;
    }

    public async Task<ICollection<ArticleViewModel>> GetAllArticlesAsync()
    {
        var articles = await _articleRepo.SelectAll()
            .Include(a=>a.Blocks)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<ICollection<ArticleViewModel>>(articles);
    }

    public async Task<ArticleViewModel> GetArticleByIdAsync(long id)
    {
        var article = await _articleRepo.SelectAll()
             .Where(a => a.Id == id)
             .Include(a => a.Blocks)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (article is null)
            throw new CyberException(404, "Article is not found");

        return _mapper.Map<ArticleViewModel>(article);
    }

    public async Task<ArticleViewModel> UpdateArticleAsync(long id,ArticlePutModel model)
    {
        var article = await _articleRepo.SelectAll()
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (article is null)
            throw new CyberException(404, "Article is not found");

        _mapper.Map(model, article);

        var orderList = model.Orders.Split(',').ToList();
        int imageIndex = 0, videoIndex = 0, textIndex = 0, codeIndex = 0;
        var blocks = new List<ContentBlock>();

        for (int i = 0; i < orderList.Count; i++)
        {
            if (orderList[i] == "text" && textIndex < model.Texts.Count)
            {
                blocks.Add(new TextBlock { Order = i, Text = model.Texts[textIndex++] });
            }
            else if (orderList[i] == "image" && imageIndex < model.Images.Count)
            {
                var uniqueFileName = await MediaHelper.UploadFile(model.Images[imageIndex], "image");
                blocks.Add(new ImageBlock { Order = i, ImageFilePath = Path.Combine("Images", uniqueFileName) });
                imageIndex++;
            }
            else if (orderList[i] == "video" && videoIndex < model.Videos.Count)
            {
                var uniqueFileName = await MediaHelper.UploadFile(model.Videos[videoIndex], "video");
                blocks.Add(new VideoBlock { Order = i, VideoFilePath = Path.Combine("Videos", uniqueFileName) });
                videoIndex++;
            }
            else if (orderList[i] == "code" && codeIndex < model.Codes.Count)
            {
                blocks.Add(new CodeBlock { Order = i, Code = model.Codes[codeIndex++] });
            }
        }

        // Delete old blocks
        foreach (var block in article.Blocks)
        {
            if (block is ImageBlock imageBlock)
            {
                var imagePath = Path.Combine(_environment.WebRootPath, imageBlock.ImageFilePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            else if (block is VideoBlock videoBlock)
            {
                var videoPath = Path.Combine(_environment.WebRootPath, videoBlock.VideoFilePath);
                if (System.IO.File.Exists(videoPath))
                {
                    System.IO.File.Delete(videoPath);
                }
            }
        }

        article.Blocks = blocks;
        article.UpdatedAt = DateTime.UtcNow;

        var result = await _articleRepo.UpdateAsync(article);

        return _mapper.Map<ArticleViewModel>(result);
    }
}
