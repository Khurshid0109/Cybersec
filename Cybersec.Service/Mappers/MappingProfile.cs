using AutoMapper;
using Cybersec.Domain.Entities;
using Cybersec.Service.ViewModels.Users;
using Cybersec.Service.ViewModels.Code;
using Cybersec.Service.ViewModels.Article;

namespace Cybersec.Service.Mappers;
public class MappingProfile:Profile
{
	public MappingProfile()
	{
		// User
		CreateMap<User, UserPostModel>().ReverseMap();
		CreateMap<User, UserPutModel>().ReverseMap();
		CreateMap<User, UserModel>().ReverseMap();

		// UserCode
		CreateMap<UserCode,UserCodePostModel>().ReverseMap();
		CreateMap<UserCode,UserCodeViewModel>().ReverseMap();

        // Article
        CreateMap<Article, ArticleViewModel>()
           .ForMember(dest => dest.Blocks, opt => opt.MapFrom(src => src.Blocks.OrderBy(b => b.Order)));
        CreateMap<ArticlePostModel, Article>();
        CreateMap<ArticlePutModel, Article>();

        // Text
        CreateMap<TextBlock, TextBlockViewModel>();
        CreateMap<TextBlockViewModel, TextBlock>();

        // Image
        CreateMap<ImageBlock, ImageBlockViewModel>();
        CreateMap<ImageBlockViewModel, ImageBlock>();

        // Video
        CreateMap<VideoBlock, VideoBlockViewModel>();
        CreateMap<VideoBlockViewModel, VideoBlock>();

        // Code
        CreateMap<CodeBlock, CodeBlockViewModel>();
        CreateMap<CodeBlockViewModel, CodeBlock>();
    }
}
