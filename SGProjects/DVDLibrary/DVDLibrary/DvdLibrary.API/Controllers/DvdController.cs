using DVDLibrary.Data;
using DVDLibrary.Models;
using DVDLibrary.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DvdLibrary.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DvdController : ApiController
    {

        [Route("dvds")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Index()
        {
            var repo = DvdRepositoryFactory.GetRepository();
            var dvds = repo.GetAllDvd();

            return Ok(dvds);
        }

        [Route("dvd")]
        [AcceptVerbs("POST")]
        public IHttpActionResult NewDvd(GetDvds dvd)
        {
            var repo = DvdRepositoryFactory.GetRepository();
            repo.SaveNew(dvd.Title, dvd.ReleaseYear, dvd.Director, dvd.Rating, dvd.Notes);

            return Ok();
        }

        [Route("dvd/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetDvdById(int id)
        {
            var repo = DvdRepositoryFactory.GetRepository();
            var dvds = repo.GetById(id);

            return Ok(dvds);
        }

        [Route("dvds/title/{title}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetDvdsByTitle(string title)
        {
            var repo = DvdRepositoryFactory.GetRepository();
            var dvds = repo.GetByTitle(title);

            return Ok(dvds);
        }

        [Route("dvds/year/{releaseYear}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByReleaseYear(int releaseYear)
        {
            var repo = DvdRepositoryFactory.GetRepository();
            var dvds = repo.GetByYear(releaseYear);

            return Ok(dvds);
        }

        [Route("dvds/director/{directorName}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByDirector(string directorName)
        {
            var repo = DvdRepositoryFactory.GetRepository();
            var dvds = repo.GetByDirector(directorName);

            return Ok(dvds);
        }

        [Route("dvds/rating/{rating}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetByRating(string rating)
        {
            var repo = DvdRepositoryFactory.GetRepository();
            var dvds = repo.GetByRating(rating);

            return Ok(dvds);
        }

        [Route("dvd/{id}")]
        [AcceptVerbs("PUT")]
        public IHttpActionResult UpdateDvd(GetDvds dvd)
        {
            var repo = DvdRepositoryFactory.GetRepository();
            repo.Edit(dvd.DvdId, dvd.Title, dvd.ReleaseYear, dvd.Director, dvd.Rating, dvd.Notes);

            return Ok();
        }

        [Route("dvd/{id}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult DeleteDvd(int id)
        {
            var repo = DvdRepositoryFactory.GetRepository();
            repo.Delete(id);

            return Ok();
        }
    }
}