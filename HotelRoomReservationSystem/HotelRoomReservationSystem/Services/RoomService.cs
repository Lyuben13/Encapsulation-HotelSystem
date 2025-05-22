using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Services
{
    public static class RoomService
    {
        private static readonly string roomFile = "data/rooms.json";

        public static void ShowAvailableRooms()
        {
            var rooms = FileHelper<Room>.Load(roomFile);

            var availableRooms = rooms
                .Where(r => r.Status.ToLower() == "available")
                .OrderBy(r => r.RoomNumber)
                .ToList();

            if (!availableRooms.Any())
            {
                Console.WriteLine("❌ No rooms available.");
                return;
            }

            Console.WriteLine("\n✅ Available Rooms:");
            foreach (var room in availableRooms)
            {
                Console.WriteLine($"Room {room.RoomNumber} | Type: {room.Type} | Price: {room.PricePerNight:C} | Cancel Fee: {room.CancellationFee:C}");
            }
        }

        public static List<Room> GetAllRooms() => FileHelper<Room>.Load(roomFile);

        public static void SaveAllRooms(List<Room> rooms) => FileHelper<Room>.Save(roomFile, rooms);
    }
}
