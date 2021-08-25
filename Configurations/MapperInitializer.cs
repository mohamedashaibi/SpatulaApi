using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SpatulaApi.Data;
using SpatulaApi.Models;

namespace SpatulaApi.Configurations
{
	public class MapperInitializer : Profile
	{
		public MapperInitializer()
		{
			CreateMap<Category, CategoryDTO>().ReverseMap();
			CreateMap<Category, CreateCategoryDTO>().ReverseMap();
			CreateMap<ApiUser, UserDTO>().ReverseMap();
			CreateMap<ApiUser, AuthUser>().ReverseMap();
			CreateMap<ApiUser, UserLoginDTO>().ReverseMap();
			CreateMap<Course, CourseDTO>().ReverseMap();
			CreateMap<Course, UpdateCourseDTO>().ReverseMap();
			CreateMap<Course, CreateCourseDTO>().ReverseMap();
			CreateMap<Course, GetCourseDTO>().ReverseMap();
			CreateMap<Lesson, LessonGetDTO>().ReverseMap();
			CreateMap<Lesson, CreateLessonDTO>().ReverseMap();
			CreateMap<Lesson, LessonListDTO>().ReverseMap();
			CreateMap<Lesson, LessonDTO>().ReverseMap();
			CreateMap<UserCourse, UserCourseDTO>().ReverseMap();
			CreateMap<UserCourse, CreateUserCourseDTO>().ReverseMap();
			


		}
	}
}
