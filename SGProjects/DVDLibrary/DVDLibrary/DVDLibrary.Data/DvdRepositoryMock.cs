using DVDLibrary.Models;
using DVDLibrary.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Data
{
    public class DvdRepositoryMock : IDvdRepository
    {
        private static List<Dvd> _dvd = new List<Dvd>();
        private static List<Director> _director = new List<Director>();
        private static List<Rating> _rating = new List<Rating>();

        static DvdRepositoryMock()
        {
            Director director = new Director();
            director.DirectorName = "Mock Director";
            director.DirectorId = 1;
            _director.Add(director);

            Rating rating = new Rating();
            rating.RatingId = 1;
            rating.RatingName = "G";
            _rating.Add(rating);

            rating = new Rating();
            rating.RatingId = 2;
            rating.RatingName = "PG";
            _rating.Add(rating);

            rating = new Rating();
            rating.RatingId = 3;
            rating.RatingName = "PG-13";
            _rating.Add(rating);

            rating = new Rating();
            rating.RatingId = 4;
            rating.RatingName = "R";
            _rating.Add(rating);

            rating = new Rating();
            rating.RatingId = 5;
            rating.RatingName = "NR";
            _rating.Add(rating);

            Dvd dvd = new Dvd();
            dvd.DvdId = 1;
            dvd.Title = "Mock Movie";
            dvd.ReleaseYear = 2020;
            dvd.Director = director;
            dvd.Rating = rating;
            dvd.Notes = "A mock movie";

            _dvd.Add(dvd);

            dvd = new Dvd();
            dvd.DvdId = 2;
            dvd.Title = "Mock Movie, the Sequel";
            dvd.ReleaseYear = 2030;
            dvd.Director = director;
            dvd.Rating = rating;
            dvd.Notes = "Sequel to Mock Movie";

            _dvd.Add(dvd);
        }

        public void Delete(int dvdId)
        {
            _dvd.RemoveAll(x => x.DvdId == dvdId);
        }

        public void Edit(int dvdId, string title, int releaseYear, string directorName, string ratingName, string notes)
        {
            foreach(var x in _dvd)
            {
                if(dvdId == x.DvdId)
                {
                    x.DvdId = dvdId;
                    x.Title = title;
                    x.ReleaseYear = releaseYear;
                    if (_director.All(y => y.DirectorName != directorName))
                    {
                        Director director = new Director();
                        director.DirectorName = directorName;
                        director.DirectorId = _director.Max(y => y.DirectorId) + 1;
                        _director.Add(director);
                        x.Director = director;
                    }
                    else
                    {
                        x.Director = _director.First(y => y.DirectorName == directorName);
                    }
                    x.Rating = _rating.First(y => y.RatingName == ratingName);
                    x.Notes = notes;

                }
            }
        }

        public List<GetDvds> GetAllDvd()
        {
            List<GetDvds> dvds = new List<GetDvds>();
            
            for(int index = 0; index < _dvd.Count(); index++)
            {
                GetDvds dvdGet = new GetDvds();

                dvdGet.DvdId = _dvd[index].DvdId;
                dvdGet.Title = _dvd[index].Title;
                dvdGet.ReleaseYear = _dvd[index].ReleaseYear;
                dvdGet.Director = _dvd[index].Director.DirectorName;
                dvdGet.Rating = _dvd[index].Rating.RatingName;
                dvdGet.Notes = _dvd[index].Notes;

                dvds.Add(dvdGet);
            }

            return dvds;
        }

        public List<GetDvds> GetByDirector(string directorName)
        {
            List<GetDvds> dvds = new List<GetDvds>();

            foreach (var x in _dvd)
            {
                if (x.Director.DirectorName == directorName)
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
            Dvd getDvdInfo = _dvd.FirstOrDefault(x => x.DvdId == dvdId);

            dvd.DvdId = dvdId;
            dvd.Title = getDvdInfo.Title;
            dvd.ReleaseYear = getDvdInfo.ReleaseYear;
            dvd.Director = getDvdInfo.Director.DirectorName;
            dvd.Rating = getDvdInfo.Rating.RatingName;
            dvd.Notes = getDvdInfo.Notes;

            return dvd;
        }

        public List<GetDvds> GetByRating(string rating)
        {
            List<GetDvds> dvds = new List<GetDvds>();

            foreach(var x in _dvd)
            {
                if(x.Rating.RatingName == rating)
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

        public List<GetDvds> GetByTitle(string title)
        {
            {
                List<GetDvds> dvds = new List<GetDvds>();

                foreach (var x in _dvd)
                {
                    if (x.Title.Contains(title))
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

        public List<GetDvds> GetByYear(int year)
        {
            {
                List<GetDvds> dvds = new List<GetDvds>();

                foreach (var x in _dvd)
                {
                    if (x.ReleaseYear == year)
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

        public List<Director> GetDirectors()
        {
            return _director;
        }

        public List<Rating> GetRatings()
        {
            return _rating;
        }

        public void SaveNew(string title, int releaseYear, string directorName, string ratingName, string notes)
        {
            Dvd dvd = new Dvd();

            dvd.DvdId = _dvd.Max(x => x.DvdId) + 1;
            dvd.Title = title;
            dvd.ReleaseYear = releaseYear;
            if(_director.All(x => x.DirectorName != directorName))
            {
                Director director = new Director();
                director.DirectorName = directorName;
                director.DirectorId = _director.Max(x => x.DirectorId) + 1;
                _director.Add(director);
                dvd.Director = director;
            }
            else
            {
                dvd.Director = _director.First(x => x.DirectorName == directorName);
            }
            dvd.Rating = _rating.First(x => x.RatingName == ratingName);
            dvd.Notes = notes;

            _dvd.Add(dvd);
        }
    }
}
