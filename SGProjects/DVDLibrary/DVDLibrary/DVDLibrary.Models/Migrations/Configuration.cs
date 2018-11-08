namespace DVDLibrary.Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DVDLibrary.Models.DvdCatalogEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DVDLibrary.Models.DvdCatalogEntities context)
        {
            context.Directors.AddOrUpdate(
                    d => d.DirectorName,
                    new Director { DirectorName = "John"},
                    new Director { DirectorName = "Jacob"},
                    new Director { DirectorName = "Jingleheimer"},
                    new Director { DirectorName = "Schmidt"}
                );

            context.Ratings.AddOrUpdate(
                r => r.RatingName,
                new Rating { RatingName = "G"},
                new Rating { RatingName = "PG"},
                new Rating { RatingName = "PG-13"},
                new Rating { RatingName = "R"},
                new Rating { RatingName = "G"}
                );

            context.SaveChanges();
            context.Dvds.AddOrUpdate(
                d => d.Title,
                new Dvd
                {
                    Title = "New Movie",
                    ReleaseYear = 2040,
                    DirectorId = context.Directors.FirstOrDefault(d => d.DirectorName == "Jacob").DirectorId,
                    RatingId = context.Ratings.FirstOrDefault(r => r.RatingName == "PG-13").RatingId,
                    Notes = "A brand new movie"
                },
                new Dvd
                {
                    Title = "Another Movie",
                    ReleaseYear = 2020,
                    DirectorId = context.Directors.FirstOrDefault(d => d.DirectorName == "Schmidt").DirectorId,
                    RatingId = context.Ratings.FirstOrDefault(r => r.RatingName == "R").RatingId,
                    Notes = "This is another movie"
                }
                );
        }
    }
}
