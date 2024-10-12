using AutoMapper;
using Cybersec.Domain.Entities;
using Cybersec.Service.DTOs.Admins;
using Cybersec.Service.DTOs.Code;
using Cybersec.Service.DTOs.Comment;
using Cybersec.Service.DTOs.Like;
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
        CreateMap<User,UserByAdminPutModel>().ReverseMap();

        //Admin
        CreateMap<Admin,AdminViewModel>().ReverseMap();
        CreateMap<Admin,AdminPostModel>().ReverseMap();
        CreateMap<Admin,AdminSettingsModel>().ReverseMap();
        CreateMap<Admin, AdminPutModel>().ReverseMap()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

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

        // Like
        CreateMap<Like,LikePostModel>().ReverseMap();
        CreateMap<Like,LikeViewModel>().ReverseMap();

        // Comment
        CreateMap<Comment,CommentPostModel>().ReverseMap();
        CreateMap<Comment,CommentViewModel>().ReverseMap();
        CreateMap<Comment,CommentPutModel>().ReverseMap();

    }
}
