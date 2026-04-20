using Microsoft.AspNetCore.Mvc;
using ConsoleApp1.Data;
using ConsoleApp1.Models;

namespace ConsoleApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetReservations(
            [FromQuery] DateTime? date, 
            [FromQuery] string? status, 
            [FromQuery] int? roomId)
        {
            var query = StaticData.Reservations.AsQueryable();

            if (date.HasValue) query = query.Where(r => r.Date.Date == date.Value.Date);
            if (!string.IsNullOrEmpty(status)) query = query.Where(r => r.Status == status);
            if (roomId.HasValue) query = query.Where(r => r.RoomId == roomId.Value);

            return Ok(query.ToList());
        }
        
        [HttpPost]
        public ActionResult<Reservation> Create([FromBody] Reservation newRes)
        {
            var room = StaticData.Rooms.FirstOrDefault(r => r.Id == newRes.RoomId);
            if (room == null) return BadRequest("Sala nie istnieje.");
            if (!room.IsActive) return BadRequest("Sala jest nieaktywna.");
            bool hasConflict = StaticData.Reservations.Any(r => 
                r.RoomId == newRes.RoomId && 
                r.Date.Date == newRes.Date.Date &&
                r.Status != "cancelled" &&
                ((newRes.StartTime >= r.StartTime && newRes.StartTime < r.EndTime) || 
                 (newRes.EndTime > r.StartTime && newRes.EndTime <= r.EndTime) ||
                 (newRes.StartTime <= r.StartTime && newRes.EndTime >= r.EndTime))
            );

            if (hasConflict) return Conflict("Sala jest już zarezerwowana в это время.");

            newRes.Id = StaticData.Reservations.Any() ? StaticData.Reservations.Max(r => r.Id) + 1 : 1;
            StaticData.Reservations.Add(newRes);

            return CreatedAtAction(nameof(GetById), new { id = newRes.Id }, newRes);
        }

        [HttpGet("{id}")]
        public ActionResult<Reservation> GetById(int id)
        {
            var res = StaticData.Reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound();
            return Ok(res);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var res = StaticData.Reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound();
            StaticData.Reservations.Remove(res);
            return NoContent();
        }
    }
}