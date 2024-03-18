using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestaurtantApi;

namespace RestaurtantApi.Controllers
{
    public class UsersController : ApiController
    {
        private RestaurtantsEntities db = new RestaurtantsEntities();

        // GET: api/Users
        [Route("api/GetUsers")]
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
       // [Route("api/GetUser")]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            var returnuserData = new
            {
                User_Id = user.UserID,
                User_name = user.Username,
                User_Mail = user.Email
            };
            return Ok(returnuserData);

        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        //[Route("api/PutUser")]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("User Updated Successfully");
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        //[Route("api/PostUser")]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok("User Created Successfully");
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        //[Route("api/DeleteUser")]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok("User Deleted Successfully");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}