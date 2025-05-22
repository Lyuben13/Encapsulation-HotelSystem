namespace HotelReservationSystem.Models
{
    public class Room
    {
        public int RoomNumber { get; set; }
        public string Type { get; set; } 
        public decimal PricePerNight { get; set; }
        public decimal CancellationFee { get; set; }
        public string Status { get; set; } = "available"; 
    }
}
