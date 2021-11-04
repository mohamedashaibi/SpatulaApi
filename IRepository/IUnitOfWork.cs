using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpatulaApi.Data;

namespace SpatulaApi.IRepository
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<Course> CourseRepo { get; }
		IGenericRepository<Category> CategoryRepo { get; }
		IGenericRepository<Advert> AdvertRepo { get; }
		IGenericRepository<Lesson> LessonRepo { get; }
		IGenericRepository<UserCourse> UserCourseRepo { get; }
		IGenericRepository<ApiUser> UserRepo { get; }
		Task Save();
	}
}
