using System;
using HotelReservationSystem.Services;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n--- Hotel Reservation System ---");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. View Rooms");
            Console.WriteLine("4. Book a Room");
            Console.WriteLine("5. Cancel Booking");
            Console.WriteLine("6. Admin Mode");
            Console.WriteLine("7. View My Profile");
            Console.WriteLine("0. Exit");

            var currentUser = UserService.GetCurrentUser();
            if (currentUser != null)
            {
                Console.WriteLine($"👤 Logged in as: {currentUser.Username}");
            }

            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1": UserService.Register(); break;
                case "2": UserService.Login(); break;
                case "3": RoomService.ShowAvailableRooms(); break;
                case "4": BookingService.BookRoom(); break;
                case "5": BookingService.CancelBooking(); break;
                case "6": AdminService.Menu(); break;
                case "7": UserService.ViewProfile(); break;
                case "0": return;
                default: Console.WriteLine("Invalid input."); break;
            }
        }
    }
}
