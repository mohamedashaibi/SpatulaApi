using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Data
{
	public class Course
	{
		[DisplayName("رقم الكورس")]
		public int Id { get; set; }
		[DisplayName("اسم الكورس")]
		public string Name { get; set; }
		[DisplayName("الصورة")]
		public string Picture { get; set; }
		[DisplayName("رقم التصنيف")]
		[ForeignKey(nameof(Category))]
		public int CategoryId { get; set; }
		[DisplayName("التكلفة")]
		public float? Cost { get; set; }
		[DisplayName("الوصف")]
		public string Description { get; set; }
		[DisplayName("مميز")]
		public bool Featured { get; set; }
		[DisplayName("الحالة")]
		public bool Status { get; set; }
		[DisplayName("تاريخ الانشاء")]
		public DateTime CreatedDate { get; set; }
		[DisplayName("تم الانشاء من قبل")]
		[ForeignKey(nameof(ApiUser))]
		public string CreatedById { get; set; }
		[DisplayName("تاريخ التحديث")]
		public DateTime? UpdatedDate { get; set; }
		[DisplayName("تم التحديث من قبل")]
		[ForeignKey(nameof(ApiUser))]
		public string UpdatedById { get; set; }
		[DisplayName("التصنيف")]
		public virtual Category Category { get; set; }
		[DisplayName("تم الانشاء من قبل")]
		public virtual ApiUser CreatedBy { get; set; }
		[DisplayName("تم التحديث من قبل")]
		public virtual ApiUser UpdatedBy { get; set; }
		[DisplayName("الدروس")]
		public virtual IList<Lesson> Lessons { get; set; }
		[DisplayName("قائمة المشتركين")]
		public virtual IList<UserCourse> UserCourses { get; set; }
	}
}
