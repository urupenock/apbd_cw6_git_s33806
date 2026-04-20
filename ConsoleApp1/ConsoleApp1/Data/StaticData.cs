using ConsoleApp1.Models;

namespace ConsoleApp1.Data
{
    public static class StaticData
    {
        public static List<Room> Rooms = new()
        {
            new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
            new Room { Id = 2, Name = "Auditorium", BuildingCode = "A", Floor = 0, Capacity = 100, HasProjector = true, IsActive = true },
            new Room { Id = 3, Name = "Small Room", BuildingCode = "B", Floor = 2, Capacity = 5, HasProjector = false, IsActive = true },
            new Room { Id = 4, Name = "Workshop 1", BuildingCode = "C", Floor = 1, Capacity = 25, HasProjector = true, IsActive = false }
        };

        public static List<Reservation> Reservations = new()
        {
            new Reservation { Id = 1, RoomId = 1, OrganizerName = "Diana Kofman", Topic = "Math 101", Date = DateTime.Parse("2026-05-10"), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), Status = "confirmed" }
        };
    }
}