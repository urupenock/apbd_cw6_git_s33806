using Microsoft.AspNetCore.Mvc;
using ConsoleApp1.Data;
using ConsoleApp1.Models;

namespace ConsoleApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetRooms(
            [FromQuery] int? minCapacity, 
            [FromQuery] bool? hasProjector, 
            [FromQuery] bool? activeOnly)
        {
            var result = StaticData.Rooms.AsQueryable();

            if (minCapacity.HasValue) result = result.Where(r => r.Capacity >= minCapacity);
            if (hasProjector.HasValue) result = result.Where(r => r.HasProjector == hasProjector);
            if (activeOnly.HasValue && activeOnly.Value) result = result.Where(r => r.IsActive);

            return Ok(result.ToList());
        }
        
        [HttpGet("{id}")]
        public ActionResult<Room> GetById(int id)
        {
            var room = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            return Ok(room);
        }
        
        [HttpGet("building/{buildingCode}")]
        public ActionResult<IEnumerable<Room>> GetByBuilding(string buildingCode)
        {
            var rooms = StaticData.Rooms.Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase));
            return Ok(rooms);
        }
        [HttpPost]
        public ActionResult<Room> Create([FromBody] Room room)
        {
            room.Id = StaticData.Rooms.Any() ? StaticData.Rooms.Max(r => r.Id) + 1 : 1;
            
            StaticData.Rooms.Add(room);
            
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Room updatedRoom)
        {
            var existingRoom = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
            if (existingRoom == null) return NotFound();
            
            existingRoom.Name = updatedRoom.Name;
            existingRoom.BuildingCode = updatedRoom.BuildingCode;
            existingRoom.Floor = updatedRoom.Floor;
            existingRoom.Capacity = updatedRoom.Capacity;
            existingRoom.HasProjector = updatedRoom.HasProjector;
            existingRoom.IsActive = updatedRoom.IsActive;

            return Ok(existingRoom);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var room = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            
            if (StaticData.Reservations.Any(res => res.RoomId == id))
            {
                return Conflict("Nie można usunąć sali, która posiada rezerwacje.");
            }

            StaticData.Rooms.Remove(room);
            return NoContent(); 
        }
    }
}