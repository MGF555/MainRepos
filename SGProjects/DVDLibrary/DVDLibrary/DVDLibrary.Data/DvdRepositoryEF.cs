using DVDLibrary.Models;
using DVDLibrary.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Data
{
    public class DvdRepositoryEF : IDvdRepository
    {
        public void Delete(int dvdId)
        {
            using(DvdCatalogEntities data = new DvdCatalogEntities())
            {
                data.Dvds.Remove(data.Dvds.FirstOrDefault(x => x.DvdId == dvdId));
                data.SaveChanges();
            }
        }

        public void Edit(int dvdId, string title, int releaseYear, string directorName, string ratingName, string notes)
        {
            using(DvdCatalogEntities data = new DvdCatalogEntities())
            {
                Dvd dvd = data.Dvds.FirstOrDefault(x => x.DvdId == dvdId);
                dvd.Title = title;
                dvd.ReleaseYear = releaseYear;
                var directorCheck = data.Directors.FirstOrDefault(
                    y =>
                    y.DirectorName == directorName
                    );
                dvd.Director = directorCheck;
                if (dvd.Director == null || dvd.Director.DirectorName == "")
                {
                    Director director = new Director();
                    director.DirectorName = directorName;
                    data.Directors.Add(director);
                    dvd.Director = director;
                }
                dvd.Rating = data.Ratings.First(x => x.RatingName == ratingName);
                dvd.Notes = notes;
                data.SaveChanges();
            }
        }

        public List<GetDvds> GetAllDvd()
        {
            List<GetDvds> dvds = new List<GetDvds>();
            using(DvdCatalogEntities data = new DvdCatalogEntities())
            {
                var getAll = (from r in data.Dvds.Include("Director").Include("Rating") select r);
                foreach(var x in getAll)
                {
                    GetDvds dvd = new GetDvds();
                    dvd.DvdId = x.DvdId;
                    dvd.Title = x.Title;
                    dvd.ReleaseYear = x.ReleaseYear;
                    dvd.Director = x.Director.DirectorName;
                    dvd.Rating = x.Rating.RatingName;
                    dvd.Notes = x.Notes;
                    dvds.Add(dvd);
                }
            }
            return dvds;
        }

        public List<GetDvds> GetByDirector(string directorName)
        {
            List<GetDvds> dvds = new List<GetDvds>();
            using (DvdCatalogEntities data = new DvdCatalogEntities())
            {
                var getByDirector = (from r in data.Dvds.Include("Director").Include("Rating") where r.Director.DirectorName == directorName select r);
                foreach (var x in getByDirector)
                {
                    GetDvds dvd = new GetDvds();
                    dvd.DvdId = x.DvdId;
                    dvd.Title = x.Title;
                    dvd.ReleaseYear = x.ReleaseYear;
                    dvd.Director = x.Director.DirectorName;
                    dvd.Rating = x.Rating.RatingName;
                    dvd.Notes = x.Notes;
                    dvds.Add(dvd);
                }
            }
            return dvds;
        }

        public GetDvds GetById(int dvdId)
        {
            GetDvds dvd = new GetDvds();
            using (DvdCatalogEntities data = new DvdCatalogEntities())
            {
                var getDvd = (from r in data.Dvds.Include("Director").Include("Rating") where r.DvdId == dvdId select r);
                foreach (var x in getDvd)
                {
                    dvd.DvdId = x.DvdId;
                    dvd.Title = x.Title;
                    dvd.ReleaseYear = x.ReleaseYear;
                    dvd.Director = x.Director.DirectorName;
                    dvd.Rating = x.Rating.RatingName;
                    dvd.Notes = x.Notes;
                }
            }
            return dvd;
        }

        public List<GetDvds> GetByRating(string rating)
        {
            {
                List<GetDvds> dvds = new List<GetDvds>();
                using (DvdCatalogEntities data = new DvdCatalogEntities())
                {
                    var getByRating = (from r in data.Dvds.Include("Director").Include("Rating") where r.Rating.RatingName == rating select r);
                    foreach (var x in getByRating)
                    {
                        GetDvds dvd = new GetDvds();
                        dvd.DvdId = x.DvdId;
                        dvd.Title = x.Title;
                        dvd.ReleaseYear = x.ReleaseYear;
                        dvd.Director = x.Director.DirectorName;
                        dvd.Rating = x.Rating.RatingName;
                        dvd.Notes = x.Notes;
                        dvds.Add(dvd);
                    }
                }
                return dvds;
            }
        }

        public List<GetDvds> GetByTitle(string title)
        {
            List<GetDvds> dvds = new List<GetDvds>();
            using (DvdCatalogEntities data = new DvdCatalogEntities())
            {
                var getByTitle = (from r in data.Dvds.Include("Director").Include("Rating") where r.Title.Contains(title) select r);
                foreach (var x in getByTitle)
                {
                    GetDvds dvd = new GetDvds();
                    dvd.DvdId = x.DvdId;
                    dvd.Title = x.Title;
                    dvd.ReleaseYear = x.ReleaseYear;
                    dvd.Director = x.Director.DirectorName;
                    dvd.Rating = x.Rating.RatingName;
                    dvd.Notes = x.Notes;
                    dvds.Add(dvd);
                }
            }
            return dvds;
        }

        public List<GetDvds> GetByYear(int year)
        {
            List<GetDvds> dvds = new List<GetDvds>();
            using (DvdCatalogEntities data = new DvdCatalogEntities())
            {
                var getByYear = (from r in data.Dvds.Include("Director").Include("Rating") where r.ReleaseYear == year select r);
                foreach (var x in getByYear)
                {
                    GetDvds dvd = new GetDvds();
                    dvd.DvdId = x.DvdId;
                    dvd.Title = x.Title;
                    dvd.ReleaseYear = x.ReleaseYear;
                    dvd.Director = x.Director.DirectorName;
                    dvd.Rating = x.Rating.RatingName;
                    dvd.Notes = x.Notes;
                    dvds.Add(dvd);
                }
            }
            return dvds;
        }

        public List<Director> GetDirectors()
        {
            List<Director> directors = new List<Director>();
            using(DvdCatalogEntities data = new DvdCatalogEntities())
            {
                var getAll = (from r in data.Directors select r);
                foreach(var x in getAll)
                {
                    Director director = new Director();
                    director.DirectorId = x.DirectorId;
                    director.DirectorName = x.DirectorName;
                    directors.Add(director);
                }
            }
            return directors;
        }

        public List<Rating> GetRatings()
        {
            List<Rating> ratings = new List<Rating>();
            using (DvdCatalogEntities data = new DvdCatalogEntities())
            {
                var getAll = (from r in data.Ratings select r);
                foreach (var x in getAll)
                {
                    Rating rating = new Rating();
                    rating.RatingId = x.RatingId;
                    rating.RatingName = x.RatingName;
                    ratings.Add(rating);
                }
            }
            return ratings;
        }

        public void SaveNew(string title, int releaseYear, string directorName, string ratingName, string notes)
        {
            Dvd dvd = new Dvd();
            using(DvdCatalogEntities data = new DvdCatalogEntities())
            {
                dvd.Director = (from x in data.Directors select x).FirstOrDefault(y => y.DirectorName == directorName);
                if(dvd.Director == null)
                {
                    Director director = new Director();
                    director.DirectorName = directorName;
                    data.Directors.Add(director);
                }
                dvd.Rating = (from r in data.Ratings select r).First(x => x.RatingName == ratingName);
                dvd.ReleaseYear = releaseYear;
                dvd.Title = title;
                dvd.Notes = notes;
                data.Dvds.Add(dvd);
                data.SaveChanges();
            }
        }
    }
}
