using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Services
{
    public static class UserService
    {
        private static readonly string userFile = "data/users.json";
        private static User? LoggedInUser;

        public static void Register()
        {
            var users = FileHelper<User>.Load(userFile);

            Console.Write("Enter username: ");
            string username = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(username) || users.Any(u => u.Username == username))
            {
                Console.WriteLine("Invalid or already used username.");
                return;
            }

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            users.Add(new User
            {
                Username = username,
                Password = password
            });

            FileHelper<User>.Save(userFile, users);
            Console.WriteLine("✅ Registration successful!");
        }

        public static void Login()
        {
            var users = FileHelper<User>.Load(userFile);

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                Console.WriteLine("❌ Invalid credentials.");
                return;
            }

            LoggedInUser = user;
            Console.WriteLine($"✅ Logged in as {user.Username}");
        }

        public static void ViewProfile()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                Console.WriteLine("❗ You must be logged in.");
                return;
            }

            Console.WriteLine($"\n👤 Profile: {currentUser.Username}");
            if (currentUser.BookingIds.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            var allBookings = FileHelper<Booking>.Load("data/bookings.json")
                .Where(b => b.Username == currentUser.Username);

            foreach (var booking in allBookings)
            {
                Console.WriteLine($"[{booking.BookingId}] Room: {booking.RoomNumber}, {booking.CheckIn:yyyy-MM-dd} to {booking.CheckOut:yyyy-MM-dd}, {booking.TotalPrice:C}, {booking.Status}");
            }
        }


        public static User? GetCurrentUser()
        {
            return LoggedInUser;
        }

        public static void Logout()
        {
            LoggedInUser = null;
        }
    }
}
