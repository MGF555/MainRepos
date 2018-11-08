using CodeChampsAI.Data;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeChampsAI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserApiController : ApiController
    {
        private static IRepository _repo;

        static UserApiController()
        {
            _repo = DIContainer.Kernel.Get<IRepository>();
        }

        [AcceptVerbs("GET")]
        [Route("UserRoles")]
        public IHttpActionResult AllRoles()
        {
            var roles = _repo.GetUserRoles();

            return Ok(roles.Select(r => new { name = r.Name, id = r.Id }).ToList());
        }

        [AcceptVerbs("PUT")]
        [Route("User/{username}/Role/{role}")]
        public IHttpActionResult UpdateUserRole(string username, string role)
        {
            _repo.UpdateUserRole(username, role);
            return Ok();
        }

        [AcceptVerbs("DELETE")]
        [Route("User/{username}")]
        public IHttpActionResult DeleteUser(string username)
        {
            _repo.DeleteUser(username);
            return Ok();
        }
    }
}
