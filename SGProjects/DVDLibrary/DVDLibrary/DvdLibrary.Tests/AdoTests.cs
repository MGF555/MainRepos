using DVDLibrary.Data;
using NUnit.Framework;
using DVDLibrary.Models.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVDLibrary.Models;

namespace DvdLibrary.Tests
{
    [TestFixture]
    public class AdoTests
    {
        [SetUp]
        public void Init()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "DvdLibrarySampleData";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Connection = cn;
                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        [Test]
        public void ADOCanLoadDvds()
        {
            var repo = new DvdRepositoryADO();
            var dvds = repo.GetAllDvd();

            Assert.AreEqual(4, dvds.Count);
        }

        [Test]
        public void ADOCanLoadDirectors()
        {
            var repo = new DvdRepositoryADO();
            var directors = repo.GetDirectors();

            Assert.AreEqual(4, directors.Count);
        }

        [Test]
        public void ADOCanLoadRatings()
        {
            var repo = new DvdRepositoryADO();
            var ratings = repo.GetRatings();

            Assert.AreEqual(5, ratings.Count);
        }

        [Test]
        public void ADOGetDvdById()
        {
            var repo = new DvdRepositoryADO();
            int id = 1;
            var dvd = repo.GetById(id);

            Assert.AreEqual(1, dvd.DvdId);
            Assert.AreEqual("That one movie", dvd.Title);
            Assert.AreEqual(1995, dvd.ReleaseYear);
            Assert.AreEqual("Jingleheimer", dvd.Director);
            Assert.AreEqual("G", dvd.Rating);
            Assert.AreEqual("It was ok", dvd.Notes);
            
        }

        [Test]
        public void ADOGetDvdByDirector()
        {
            var repo = new DvdRepositoryADO();
            string director = "John";
            var dvd = repo.GetByDirector(director);

            Assert.AreEqual(1, dvd.Count());

            Assert.AreEqual(2, dvd[0].DvdId);
            Assert.AreEqual("That old movie", dvd[0].Title);
            Assert.AreEqual(1444, dvd[0].ReleaseYear);
            Assert.AreEqual("John", dvd[0].Director);
            Assert.AreEqual("PG-13", dvd[0].Rating);
            Assert.AreEqual("Movie did not age well", dvd[0].Notes);
        }

        [Test]
        public void ADOGetDvdByRating()
        {
            var repo = new DvdRepositoryADO();
            string rating = "R";
            var dvd = repo.GetByRating(rating);

            Assert.AreEqual(2, dvd.Count());

            Assert.AreEqual(3, dvd[0].DvdId);
            Assert.AreEqual("That other movie", dvd[0].Title);
            Assert.AreEqual(2030, dvd[0].ReleaseYear);
            Assert.AreEqual("Jacob", dvd[0].Director);
            Assert.AreEqual("R", dvd[0].Rating);
            Assert.AreEqual("A ways off", dvd[0].Notes);

            Assert.AreEqual(4, dvd[1].DvdId);
            Assert.AreEqual("A movie", dvd[1].Title);
            Assert.AreEqual(2017, dvd[1].ReleaseYear);
            Assert.AreEqual("Schmidt", dvd[1].Director);
            Assert.AreEqual("R", dvd[1].Rating);
            Assert.AreEqual("Eh, no comment", dvd[1].Notes);
        }

        [Test]
        public void ADOGetDvdByTitle()
        {
            var repo = new DvdRepositoryADO();
            string title = "That other movie";
            var dvd = repo.GetByTitle(title);

            Assert.AreEqual(1, dvd.Count());

            Assert.AreEqual(3, dvd[0].DvdId);
            Assert.AreEqual("That other movie", dvd[0].Title);
            Assert.AreEqual(2030, dvd[0].ReleaseYear);
            Assert.AreEqual("Jacob", dvd[0].Director);
            Assert.AreEqual("R", dvd[0].Rating);
            Assert.AreEqual("A ways off", dvd[0].Notes);
        }

        [Test]
        public void ADOGetDvdByYear()
        {
            var repo = new DvdRepositoryADO();
            int year = 1444;
            var dvd = repo.GetByYear(year);

            Assert.AreEqual(1, dvd.Count());

            Assert.AreEqual(2, dvd[0].DvdId);
            Assert.AreEqual("That old movie", dvd[0].Title);
            Assert.AreEqual(1444, dvd[0].ReleaseYear);
            Assert.AreEqual("John", dvd[0].Director);
            Assert.AreEqual("PG-13", dvd[0].Rating);
            Assert.AreEqual("Movie did not age well", dvd[0].Notes);
        }

        [Test]
        public void ADOCanAddDvd()
        {
            Dvd dvdToAdd = new Dvd();
            var repo = new DvdRepositoryADO();

            string title = "Brand new movie";
            int releaseYear = 2017;
            string directorName = "NewDirector";
            string ratingName = "PG";
            string notes = "It's brand new";

            repo.SaveNew(title, releaseYear, directorName, ratingName, notes);

            Assert.AreEqual(5, repo.GetAllDvd().Count());
        }

        [Test]
        public void ADOCanDeleteDvd()
        {
            var repo = new DvdRepositoryADO();
            var dvds = repo.GetAllDvd();
            Assert.AreEqual(4, dvds.Count());
            repo.Delete(4);
            dvds = repo.GetAllDvd();
            Assert.AreEqual(3, dvds.Count());
        }

        [Test]
        public void ADOCanEditDvd()
        {
            var repo = new DvdRepositoryADO();
            int dvdId = 1;
            string title = "Updated Dvd";
            int releaseYear = 1990;
            string directorName = "Jacob";
            string ratingName = "PG-13";
            string notes = "This has been updated";

            repo.Edit(dvdId, title, releaseYear, directorName, ratingName, notes);

            var updatedDvd = repo.GetById(1);

            Assert.AreEqual(1, updatedDvd.DvdId);
            Assert.AreEqual("Updated Dvd", updatedDvd.Title);
            Assert.AreEqual(1990, updatedDvd.ReleaseYear);
            Assert.AreEqual("Jacob", updatedDvd.Director);
            Assert.AreEqual("PG-13", updatedDvd.Rating);
            Assert.AreEqual("This has been updated", updatedDvd.Notes);
        }
    }
}
