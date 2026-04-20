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
    }
}