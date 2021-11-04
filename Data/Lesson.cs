using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Data
{
	public class Lesson
	{
		[DisplayName("رقم الدرس")]
		public int Id { get; set; }
		[DisplayName("تحذيرات")]
		public string Warnings { get; set; }
		[DisplayName("الاسم بالعربي")]
		public string ArabicName { get; set; }
		[DisplayName("الوصف")]
		public string Description { get; set; }
		[DisplayName("الادوات المستخدمة")]
		public string Utensils { get; set; }
		[DisplayName("المفادير")]
		public string Ingredients { get; set; }
		[DisplayName("الفيديو")]
		public string VideoUrl { get; set; }
		[DisplayName("الحالة")]
		public bool Status { get; set; }
		[DisplayName("تاريخ الانشاء")]
		public DateTime CreatedDate { get; set; }
		[DisplayName("تم الانشاء من قبل")]
		[ForeignKey(nameof(ApiUser))]
		public string CreatedById { get; set; }
		[DisplayName("الكورس")]
		[ForeignKey(nameof(Course))]
		public int CourseId { get; set; }
		[DisplayName("الترتيب")]
		public int Order { get; set; }
		[DisplayName("تم الانشاء من قبل")]
		public virtual ApiUser CreatedBy { get; set; }
		[DisplayName("الكورس")]
		public virtual Course Course { get; set; }
	}
}
