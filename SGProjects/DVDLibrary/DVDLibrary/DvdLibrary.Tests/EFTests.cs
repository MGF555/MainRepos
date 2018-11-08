using DVDLibrary.Data;
using DVDLibrary.Models;
using DVDLibrary.Models.Queries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvdLibrary.Tests
{
    [TestFixture]
    public class EFTests
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
        public void EFCanLoadDvds()
        {
            var repo = new DvdRepositoryEF();
            var dvds = repo.GetAllDvd();

            Assert.AreEqual(4, dvds.Count());
        }

        [Test]
        public void EFCanGetDvdById()
        {
            var repo = new DvdRepositoryEF();
            var dvd = repo.GetById(2);

            Assert.AreEqual(2, dvd.DvdId);
            Assert.AreEqual("That old movie", dvd.Title);
            Assert.AreEqual(1444, dvd.ReleaseYear);
        }

        [Test]
        public void EFCanGetByTitle()
        {
            var repo = new DvdRepositoryEF();
            var dvds = repo.GetByTitle("A Movie");

            Assert.AreEqual(1, dvds.Count());

            var nullDvd = repo.GetByTitle("None");
            Assert.AreEqual(0, nullDvd.Count());
        }

        [Test]
        public void EFCanGetByRating()
        {
            var repo = new DvdRepositoryEF();
            var dvds = repo.GetByRating("G");

            Assert.AreEqual(1, dvds.Count());

            var dvd = repo.GetByRating("PG-13");

            Assert.AreEqual(1, dvd.Count());
        }

        [Test]
        public void EFCanGetByDirector()
        {
            var repo = new DvdRepositoryEF();
            var dvds = repo.GetByDirector("John");

            Assert.AreEqual(1, dvds.Count());

            var dvd = repo.GetByDirector("Jacob");
            Assert.AreEqual(1, dvd.Count());
        }

        [Test]
        public void EFCanGetByYear()
        {
            var repo = new DvdRepositoryEF();
            var dvds = repo.GetByYear(2030);

            Assert.AreEqual(1, dvds.Count());
        }

        [Test]
        public void EFCanLoadDirectors()
        {
            var repo = new DvdRepositoryEF();
            var directors = repo.GetDirectors();

            Assert.AreEqual(4, directors.Count());
        }

        [Test]
        public void EFCanLoadRatings()
        {
            var repo = new DvdRepositoryEF();
            var ratings = repo.GetRatings();

            Assert.AreEqual(5, ratings.Count());
        }

        [Test]
        public void EFCanSaveNewDvd()
        {
            var repo = new DvdRepositoryEF();
            string title = "A new dvd";
            int releaseYear = 2017;
            string directorName = "New Director";
            string ratingName = "PG-13";
            string notes = "This is a brand new dvd";

            repo.SaveNew(title, releaseYear, directorName, ratingName, notes);

            var dvds = repo.GetAllDvd();

            Assert.AreEqual(5, dvds.Count());
        }

        [Test]
        public void EFCanDeleteDvd()
        {
            var repo = new DvdRepositoryEF();
            int dvdId = 4;
            repo.Delete(dvdId);

            Assert.AreEqual(3, repo.GetAllDvd().Count());
        }

        [Test]
        public void EFCanEditDvd()
        {
            var repo = new DvdRepositoryEF();
            int dvdId = 2;
            string title = "Edited dvd";
            int releaseYear = 1900;
            string directorName = "Edited director";
            string ratingName = "R";
            string notes = "This has been edited";

            repo.Edit(dvdId, title, releaseYear, directorName, ratingName, notes);
            GetDvds dvd = repo.GetById(2);

            Assert.AreEqual(2, dvd.DvdId);
            Assert.AreEqual(title, dvd.Title);
        }
    }
}
