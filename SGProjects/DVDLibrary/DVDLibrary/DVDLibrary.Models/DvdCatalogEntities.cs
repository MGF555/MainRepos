using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Models
{
    public class DvdCatalogEntities :DbContext
    {
        public DvdCatalogEntities()
            : base("DefaultConnection")
        {

        }

        public DbSet<Dvd> Dvds { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Director> Directors { get; set; }
    }
}
