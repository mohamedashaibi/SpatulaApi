using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SpatulaApi.Data
{
	public class Review
	{
		[DisplayName("رقم التقييم")]
		public int Id { get; set; }
		[DisplayName("سرعة التطبيق")]
		public short AppSpeed { get; set; }
		[DisplayName("ميزة التنزيل")]
		public short DownloadQuality { get; set; }
		[DisplayName("الدفع الالكتروني")]
		public short PaymentQuality { get; set; }
		[DisplayName("سهولة التنقل داخل التطبيق")]
		public short EaseOfUse { get; set; }
		[DisplayName("تعليقات اخرى")]
		public string Notes { get; set; }
	}
}
