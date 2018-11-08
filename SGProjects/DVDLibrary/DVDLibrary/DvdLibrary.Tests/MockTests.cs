using DVDLibrary.Data;
using DVDLibrary.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvdLibrary.Tests
{
    [TestFixture]
    public class MockTests
    {
        [Test]
        public void MockCanLoadDvds()
        {
            var repo = new DvdRepositoryMock();
            var test = repo.GetAllDvd();

            Assert.AreEqual(2, test.Count());
        }

        [Test]
        public void MockCanLoadDirector()
        {
            var repo = new DvdRepositoryMock();
            var test = repo.GetDirectors();
            Assert.AreEqual(3, test.Count());
        }

        [Test]
        public void MockCanLoadRating()
        {
            var repo = new DvdRepositoryMock();
            var test = repo.GetRatings();
            Assert.AreEqual(5, test.Count());
        }

        [Test]
        public void MockCanLoadById()
        {
            var repo = new DvdRepositoryMock();
            int dvdId = (2);
            var dvd = repo.GetById(dvdId);
            Assert.AreEqual(2, dvd.DvdId);
        }

        [Test]
        public void MockCanLoadByRating()
        {
            var repo = new DvdRepositoryMock();
            string rating = "NR";
            var dvds = repo.GetByRating(rating);
            Assert.AreEqual(1, dvds.Count());
        }

        [Test]
        public void MockCanLoadByDirectior()
        {
            var repo = new DvdRepositoryMock();
            string directorName = "Mock Director";
            var dvds = repo.GetByDirector(directorName);
            Assert.AreEqual(1, dvds.Count());
        }

        [Test]
        public void MockCanLoadByTitle()
        {
            var repo = new DvdRepositoryMock();
            string title = "Mock Movie";
            var dvds = repo.GetByTitle(title);
            Assert.AreEqual(1, dvds.Count());
        }

        [Test]
        public void MockCanLoadByYear()
        {
            var repo = new DvdRepositoryMock();
            int releaseYear = 2020;
            var dvds = repo.GetByYear(releaseYear);
            Assert.AreEqual(1, dvds.Count());
        }

        [Test]
        public void MockCanAddDvd()
        {
            var repo = new DvdRepositoryMock();

            string title = "A new movie";
            int releaseYear = 2000;
            string directorName = "John";
            string ratingName = "G";
            string notes = "A note";

            repo.SaveNew(title, releaseYear, directorName, ratingName, notes);
            var test = repo.GetAllDvd();

            Assert.AreEqual(3, test.Count());
        }

        [Test]
        public void MockCanDeleteDvd()
        {
            var repo = new DvdRepositoryMock();
            var test = repo.GetAllDvd();
            Assert.AreEqual(3, test.Count());
            repo.Delete(3);
            test = repo.GetAllDvd();

            Assert.AreEqual(2, test.Count());
        }

        [Test]
        public void MockCanEditDvd()
        {
            var repo = new DvdRepositoryMock();
            int dvdId = 2;
            string title = "An edited movie";
            int releaseYear = 1999;
            string directorName = "Jacob";
            string ratingName = "PG";
            string notes = "A note";
            repo.Edit(dvdId, title, releaseYear, directorName, ratingName, notes);

            var test = repo.GetAllDvd();
            var editedDvd = repo.GetById(2);

            Assert.AreEqual(2, editedDvd.DvdId);
            Assert.AreEqual("An edited movie", editedDvd.Title);
            Assert.AreEqual(1999, editedDvd.ReleaseYear);
            Assert.AreEqual("Jacob", editedDvd.Director);
            Assert.AreEqual("PG", editedDvd.Rating);
        }
    }
}
