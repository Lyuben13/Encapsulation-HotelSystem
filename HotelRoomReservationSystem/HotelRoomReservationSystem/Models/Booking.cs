namespace HotelReservationSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string Username { get; set; }
        public int RoomNumber { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "confirmed"; 
    }
}
