using AutoMapper;
using Cybersec.Domain.Entities;
using Cybersec.Service.DTOs.Admins;
using Cybersec.Service.DTOs.Code;
using Cybersec.Service.DTOs.Users;
using Cybersec.Service.ViewModels.Article;

namespace Cybersec.Service.Mappers;
public class MappingProfile:Profile
{
	public MappingProfile()
	{
		// User
		CreateMap<User, UserPostModel>().ReverseMap();
		CreateMap<User, UserPutModel>().ReverseMap();
		CreateMap<User, UserViewModel>().ReverseMap();

        CreateMap<Admin,AdminViewModel>().ReverseMap();

		// UserCode
		CreateMap<UserCode,UserCodePostModel>().ReverseMap();
		CreateMap<UserCode,UserCodeViewModel>().ReverseMap();

        // Article
        CreateMap<Article, ArticleViewModel>()
           .ForMember(dest => dest.Blocks, opt => opt.MapFrom(src => src.Blocks.OrderBy(b => b.Order)))
           .ReverseMap();

        CreateMap<ArticlePostModel, Article>()
             .ForMember(dest => dest.Blocks, opt => opt.Ignore());

        CreateMap<ArticlePutModel, Article>()
            .ForMember(dest => dest.Blocks, opt => opt.Ignore());

        // Content block
        CreateMap<ContentBlock, ContentBlockViewModel>()
            .IncludeAllDerived()
            .ReverseMap();

        // Text
        CreateMap<TextBlock, TextBlockViewModel>().ReverseMap();

        // Image
        CreateMap<ImageBlock, ImageBlockViewModel>().ReverseMap();

        // Video
        CreateMap<VideoBlock, VideoBlockViewModel>().ReverseMap();

        // Code
        CreateMap<CodeBlock, CodeBlockViewModel>().ReverseMap();
    }
}
