using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Data
{
	public class UserCourse
	{
		[DisplayName("رقم التعريف")]
		public int Id { get; set; }
		[DisplayName("الكورس")]
		[ForeignKey(nameof(Course))]
		public int CourseId { get; set; }
		[DisplayName("المستخدم")]
		[ForeignKey(nameof(ApiUser))]
		public string UserId { get; set; }
		[DisplayName("تاريخ الاضافة")]
		public DateTime AddedDate { get; set; }
		[DisplayName("الدرس الواصل اليه")]
		[ForeignKey(nameof(Lesson))]
		public int? LessonReachedId { get; set; }
		[DisplayName("المستخدم")]
		public virtual ApiUser User { get; set; }
		[DisplayName("الكورس")]
		public virtual Course Course { get; set; }
		[DisplayName("الدرس الواصل اليه")]
		public virtual Lesson LessonReached { get; set; }
	}
}
