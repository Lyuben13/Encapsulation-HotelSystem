using HotelReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelReservationSystem.Services
{
    public static class BookingService
    {
        private static readonly string bookingFile = "data/bookings.json";

        public static void BookRoom()
        {
            var currentUser = UserService.GetCurrentUser();
            if (currentUser == null)
            {
                Console.WriteLine("❗ You must be logged in to book a room.");
                return;
            }

            var rooms = RoomService.GetAllRooms();
            var bookings = FileHelper<Booking>.Load(bookingFile);

            Console.Write("Enter room type (e.g. Deluxe, Suite): ");
            string roomType = Console.ReadLine()?.Trim();

            var availableRooms = rooms
                .Where(r => r.Type.Equals(roomType, StringComparison.OrdinalIgnoreCase) && r.Status == "available")
                .ToList();

            if (!availableRooms.Any())
            {
                Console.WriteLine("❌ No available rooms of that type.");
                return;
            }

            Console.WriteLine("\nAvailable Rooms:");
            foreach (var room in availableRooms)
            {
                Console.WriteLine($"Room {room.RoomNumber} - {room.Type} - {room.PricePerNight:C}");
            }

            Console.Write("Enter room number to book: ");
            if (!int.TryParse(Console.ReadLine(), out int roomNumber))
            {
                Console.WriteLine("❌ Invalid room number.");
                return;
            }

            var selectedRoom = availableRooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (selectedRoom == null)
            {
                Console.WriteLine("❌ Room not available.");
                return;
            }

            Console.Write("Enter check-in date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkIn)) return;

            Console.Write("Enter check-out date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkOut)) return;

            if (checkOut <= checkIn)
            {
                Console.WriteLine("❌ Check-out must be after check-in.");
                return;
            }

            int nights = (checkOut - checkIn).Days;
            decimal totalPrice = nights * selectedRoom.PricePerNight;

            int bookingId = bookings.Any() ? bookings.Max(b => b.BookingId) + 1 : 1001;

            var newBooking = new Booking
            {
                BookingId = bookingId,
                Username = currentUser.Username,
                RoomNumber = selectedRoom.RoomNumber,
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalPrice = totalPrice,
                Status = "confirmed"
            };

            bookings.Add(newBooking);
            FileHelper<Booking>.Save(bookingFile, bookings);

            selectedRoom.Status = "booked";
            RoomService.SaveAllRooms(rooms);

            var allUsers = FileHelper<User>.Load("users.json");
            var user = allUsers.First(u => u.Username == currentUser.Username);
            user.BookingIds.Add(bookingId);
            FileHelper<User>.Save("users.json", allUsers);

            Console.WriteLine($"✅ Booking successful! Booking ID: {bookingId}");
        }

        public static void CancelBooking()
        {
            var currentUser = UserService.GetCurrentUser();
            if (currentUser == null)
            {
                Console.WriteLine("❗ You must be logged in to cancel a booking.");
                return;
            }

            var bookings = FileHelper<Booking>.Load(bookingFile);
            var rooms = RoomService.GetAllRooms();

            Console.Write("Enter your Booking ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("❌ Invalid booking ID.");
                return;
            }

            var booking = bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                Console.WriteLine("❌ Booking not found.");
                return;
            }

            if (booking.Username != currentUser.Username)
            {
                Console.WriteLine("❌ You can only cancel your own bookings.");
                return;
            }

            if (booking.Status == "cancelled")
            {
                Console.WriteLine("ℹ️ This booking has already been cancelled.");
                return;
            }

            booking.Status = "cancelled";

            var room = rooms.FirstOrDefault(r => r.RoomNumber == booking.RoomNumber);
            if (room != null)
            {
                room.Status = "available";
                Console.WriteLine($"\n❗ Cancellation Fee: {room.CancellationFee:C}");
            }

            FileHelper<Booking>.Save(bookingFile, bookings);
            RoomService.SaveAllRooms(rooms);

            Console.WriteLine("✅ Booking cancelled successfully.");
        }
    }
}
