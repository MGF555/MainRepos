using DVDLibrary.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Models
{
    public interface IDvdRepository
    {
        List<GetDvds> GetAllDvd();
        GetDvds GetById(int dvdId);
        List<Director> GetDirectors();
        List<Rating> GetRatings();
        List<GetDvds> GetByTitle(string title);
        List<GetDvds> GetByYear(int year);
        List<GetDvds> GetByDirector(string directorName);
        List<GetDvds> GetByRating(string rating);
        void SaveNew(string title, int releaseYear, string directorName, string ratingName, string notes);
        void Edit(int dvdId, string title, int releaseYear, string directorName, string ratingName, string notes);
        void Delete(int dvdId);
    }
}
