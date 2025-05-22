using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservationSystem.Models;
using HotelReservationSystem.Utils;
//using HotelReservationSystem.Utils;

namespace HotelReservationSystem.Services
{
    public static class AdminService
    {
        private static readonly string bookingFile = "data/bookings.json";
        private static readonly string roomFile = "data/rooms.json";

        public static void Menu()
        {
            Console.Write("Enter admin password: ");
            string input = Console.ReadLine();

            if (input != "admin123")
            {
                Console.WriteLine("❌ Invalid password.");
                return;
            }

            while (true)
            {
                Console.WriteLine("\n🔐 Admin Panel");
                Console.WriteLine("1. View All Bookings");
                Console.WriteLine("2. View Total Income");
                Console.WriteLine("3. View Total Cancellation Fees");
                Console.WriteLine("4. Add a Room");
                Console.WriteLine("5. Remove a Room");
                Console.WriteLine("6. Backup All Data");
                Console.WriteLine("7. Search Bookings");
                Console.WriteLine("0. Return");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAllBookings(); break;
                    case "2": ViewTotalIncome(); break;
                    case "3": ViewCancellationFees(); break;
                    case "4": AddRoom(); break;
                    case "5": RemoveRoom(); break;
                    case "6": BackupService.BackupAllData(); break;
                    case "7": SearchBookings(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        private static void SearchBookings()
        {
            var bookings = FileHelper<Booking>.Load(bookingFile);
            Console.WriteLine("\nSearch by:\n1. Username\n2. Room Number\n3. Date Range");
            Console.Write("Enter option: ");
            string option = Console.ReadLine();

            IEnumerable<Booking> result = new List<Booking>();

            switch (option)
            {
                case "1":
                    Console.Write("Enter username: ");
                    string uname = Console.ReadLine();
                    result = bookings.Where(b => b.Username.Equals(uname, StringComparison.OrdinalIgnoreCase));
                    break;
                case "2":
                    Console.Write("Enter room number: ");
                    if (int.TryParse(Console.ReadLine(), out int room))
                        result = bookings.Where(b => b.RoomNumber == room);
                    break;
                case "3":
                    Console.Write("Start date (yyyy-mm-dd): ");
                    DateTime.TryParse(Console.ReadLine(), out DateTime start);
                    Console.Write("End date (yyyy-mm-dd): ");
                    DateTime.TryParse(Console.ReadLine(), out DateTime end);
                    result = bookings.Where(b => b.CheckIn >= start && b.CheckOut <= end);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    return;
            }

            foreach (var b in result)
            {
                Console.WriteLine($"[{b.BookingId}] {b.Username} | Room: {b.RoomNumber} | {b.CheckIn:yyyy-MM-dd} to {b.CheckOut:yyyy-MM-dd} | {b.TotalPrice:C} | {b.Status}");
            }

            if (!result.Any())
                Console.WriteLine("No matching bookings found.");
        }


        private static void ViewAllBookings()
        {
            var bookings = FileHelper<Booking>.Load(bookingFile);
            if (!bookings.Any())
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            foreach (var b in bookings)
            {
                Console.WriteLine($"[{b.BookingId}] {b.Username} | Room: {b.RoomNumber} | {b.CheckIn:yyyy-MM-dd} to {b.CheckOut:yyyy-MM-dd} | {b.TotalPrice:C} | {b.Status}");
            }
        }

        private static void ViewTotalIncome()
        {
            var bookings = FileHelper<Booking>.Load(bookingFile);
            var income = bookings
                .Where(b => b.Status == "confirmed")
                .Sum(b => b.TotalPrice);

            Console.WriteLine($"💰 Total Income from Confirmed Bookings: {income:C}");
        }

        private static void ViewCancellationFees()
        {
            var bookings = FileHelper<Booking>.Load(bookingFile);
            var rooms = FileHelper<Room>.Load(roomFile);

            decimal totalFees = bookings
                .Where(b => b.Status == "cancelled")
                .Join(rooms,
                      b => b.RoomNumber,
                      r => r.RoomNumber,
                      (b, r) => r.CancellationFee)
                .Sum();

            Console.WriteLine($"💸 Total Cancellation Fees Collected: {totalFees:C}");
        }

        private static void AddRoom()
        {
            var rooms = FileHelper<Room>.Load(roomFile);

            Console.Write("Enter Room Number: ");
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("❌ Invalid number.");
                return;
            }

            Console.Write("Enter Room Type: ");
            string type = Console.ReadLine();

            Console.Write("Enter Price per Night: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("❌ Invalid price.");
                return;
            }

            Console.Write("Enter Cancellation Fee: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal cancelFee))
            {
                Console.WriteLine("❌ Invalid fee.");
                return;
            }

            rooms.Add(new Room
            {
                RoomNumber = number,
                Type = type,
                PricePerNight = price,
                CancellationFee = cancelFee,
                Status = "available"
            });

            FileHelper<Room>.Save(roomFile, rooms);
            Console.WriteLine("✅ Room added.");
        }

        private static void RemoveRoom()
        {
            var rooms = FileHelper<Room>.Load(roomFile);

            Console.Write("Enter Room Number to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("❌ Invalid number.");
                return;
            }

            var room = rooms.FirstOrDefault(r => r.RoomNumber == number);
            if (room == null)
            {
                Console.WriteLine("❌ Room not found.");
                return;
            }

            rooms.Remove(room);
            FileHelper<Room>.Save(roomFile, rooms);
            Console.WriteLine("✅ Room removed.");
        }
    }
}
