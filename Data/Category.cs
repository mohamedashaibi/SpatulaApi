using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Data
{
	public class Category
	{
		[DisplayName("رقم التصنيف")]
		public int Id { get; set; }
		[DisplayName("الاسم العربي")]
		public string ArabicName { get; set; }
		[DisplayName("الاسم الانجليزي")]
		public string EnglishName { get; set; }
		[DisplayName("الحالة")]
		public bool Status { get; set; }
		[DisplayName("قائمة الكورسات")]
		public virtual IList<Course> Courses { get; set; }
	}
}
