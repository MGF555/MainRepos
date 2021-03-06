﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeChampsAI.Models
{
	public class NewPostViewModel
	{
		[AllowHtml]
		[Display(Name = "PostId")]
		public int PostId { get; set; }

		[AllowHtml]
		[Display(Name = "Subject")]
		public String Subject { get; set; }

		[AllowHtml]
		[Display(Name = "Body")]
		public String Body { get; set; }

		[AllowHtml]
		[Display(Name = "ListOfTags")]
		public string ListOfTags { get; set; }
	}
}