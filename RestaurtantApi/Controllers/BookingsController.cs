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
    public class BookingsController : ApiController
    {
        private RestaurtantsEntities db = new RestaurtantsEntities();
        // GET: api/Bookings
        [Route("api/GetBookings")]
        public IQueryable<Booking> GetBookings()
        {
            return db.Bookings;
        }

        // GET: api/Bookings/5
        //[Route("api/GetBooking")]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult GetBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            var returnbookingData = new{
                Booking_Id = booking.BookingID,
                Booking_Date = booking.BookingDate,
                Start_time = booking.StartTime,
                End_time = booking.EndTime,
                Purpose = booking.Purpose,
                Status = booking.Status
            };
            return Ok(returnbookingData);
        }

        // PUT: api/Bookings/5
        [Route("api/PutBooking")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Booking Updated Successfully");
        }

        // POST: api/Bookings
        [Route("api/PostBooking")]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult PostBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.BookingID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Booking Created Successfully");
        }

        // DELETE: api/Bookings/5
        [ResponseType(typeof(Booking))]
        [Route("api/DeleteBooking")]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok("Booking Deleted Successfully");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }
    }
}