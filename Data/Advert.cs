using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Data
{
	public class Advert
	{
		[DisplayName("رقم الاعلان")]
		public int Id { get; set; }
		[DisplayName("الرابط")]
		public string Url { get; set; }
		[DisplayName("الصورة")]
		public string Image { get; set; }
		[DisplayName("الترتيب")]
		public int Order { get; set; }
		[DisplayName("الحالة")]
		public bool Status { get; set; }
	}
}
