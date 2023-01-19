﻿using System.ComponentModel.DataAnnotations;

namespace Kenny.Services.Email.Models
{
	public class EmailLog
	{
		[Key]
		public int Id { get; set; }
		public string Email { get; set; }
		public string Log { get; set; }
		public DateTime EmailSent { get; set; }
	}
}
