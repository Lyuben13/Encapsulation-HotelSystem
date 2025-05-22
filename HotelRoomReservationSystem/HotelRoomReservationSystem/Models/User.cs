namespace HotelReservationSystem.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<int> BookingIds { get; set; } = new();
    }
}
