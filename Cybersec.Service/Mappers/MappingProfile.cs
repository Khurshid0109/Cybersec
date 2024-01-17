using AutoMapper;
using Cybersec.Domain.Entities;
using Cybersec.Service.ViewModels.News;
using Cybersec.Service.ViewModels.Codes;
using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Service.Mappers;
public class MappingProfile:Profile
{
	public MappingProfile()
	{
		// User
		CreateMap<User, UserPostModel>().ReverseMap();
		CreateMap<User, UserPutModel>().ReverseMap();
		CreateMap<User, UserModel>().ReverseMap();

		// News
		CreateMap<News,NewsPostModel>().ReverseMap();
		CreateMap<News,NewsPutModel>().ReverseMap();
		CreateMap<News,NewsModel>().ReverseMap();

		// UserCode
		CreateMap<UserCode,UserCodePostModel>().ReverseMap();
		CreateMap<UserCode,UserCodeViewModel>().ReverseMap();
	}
}
