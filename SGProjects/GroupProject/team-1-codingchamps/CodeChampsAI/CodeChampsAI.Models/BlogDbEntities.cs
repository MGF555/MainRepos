using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChampsAI.Models
{
	public class BlogDbEntities : DbContext
	{
		public BlogDbEntities()
			: base("AIBlog")
		{
		}

		public DbSet<Post> Posts { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<StaticPage> StaticPages { get; set; }

	}
}
