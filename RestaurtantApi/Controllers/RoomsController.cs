using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RestaurtantApi;

namespace RestaurtantApi.Controllers
{
    public class RoomsController : ApiController
    {
        private RestaurtantsEntities db = new RestaurtantsEntities();

        // GET: api/Rooms
        [ResponseType(typeof(IEnumerable<Room>))]
        [Route("api/GetRooms")]
        public IQueryable<Room> GetRooms()
        {
            return db.Rooms;
        }

        // GET: api/Rooms/5
        [ResponseType(typeof(Room))]
        //[Route("api/GetRoom")]
        public IHttpActionResult GetRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            var returnRoomData = new
            {
                RoomId = room.RoomID,
                RoomName = room.RoomName,
                Capacity = room.Capacity,
                RemainingSpace = room.RemainingSpace,
                Description = room.Description
            };

            return Ok(returnRoomData);
        }

        // PUT: api/Rooms/5
       // [Route("api/PutRoom")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoom(int id, Room room)
        {
            if (room == null)
            {
                return BadRequest("Room object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.RoomID)
            {
                return BadRequest("ID in the request body does not match the ID in the URL");
            }

            try
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Room Updated Successfully");
        }



        // POST: api/Rooms
        [ResponseType(typeof(Room))]
        //[Route("api/PostRoom")]
        public IHttpActionResult PostRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RoomExists(room.RoomID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Room Created Successfully");
        }

        // DELETE: api/Rooms/5
       // [Route("api/DeleteRoom")]
        [ResponseType(typeof(Room))]

        public IHttpActionResult DeleteRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            // Remove related Users
            var usersInRoom = db.Users.Where(u => u.UserID == id);
            var bookingInRoom = db.Bookings.Where(b => b.BookingID == id);
            foreach (var user in usersInRoom)
            {
                db.Users.Remove(user);
            }
            foreach (var booking in bookingInRoom)
            {
                db.Bookings.Remove(booking);
            }

            // Remove the room itself
            db.Rooms.Remove(room);
            db.SaveChanges();

            return Ok("Room Deleted Successfully");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Route("api/RoomExists")]
        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.RoomID == id) > 0;
        }
    }
}