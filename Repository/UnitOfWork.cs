using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpatulaApi.Data;
using SpatulaApi.IRepository;

namespace SpatulaApi.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DatabaseContext _context;
		private IGenericRepository<Course> _courses;
		private IGenericRepository<Advert> _adverts;
		private IGenericRepository<Category> _categories;
		private IGenericRepository<Lesson> _lessons;
		private IGenericRepository<UserCourse> _userCourses;
		private IGenericRepository<ApiUser> _user;

		public UnitOfWork(DatabaseContext context)
		{
			_context = context;
		}
		public IGenericRepository<Course> CourseRepo => _courses ??= new GenericRepository<Course>(_context);

		public IGenericRepository<Category> CategoryRepo => _categories ??= new GenericRepository<Category>(_context);

		public IGenericRepository<Lesson> LessonRepo => _lessons ??= new GenericRepository<Lesson>(_context);

		public IGenericRepository<UserCourse> UserCourseRepo => _userCourses ??= new GenericRepository<UserCourse>(_context);

		public IGenericRepository<ApiUser> UserRepo => _user ??= new GenericRepository<ApiUser>(_context);
		public IGenericRepository<Advert> AdvertRepo => _adverts ??= new GenericRepository<Advert>(_context);

		public void Dispose()
		{
			_context.Dispose();
			GC.SuppressFinalize(this);
		}


		public async Task Save()
		{
			await _context.SaveChangesAsync();
		}
	}
}
