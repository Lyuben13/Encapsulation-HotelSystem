namespace HotelReservationSystem.Models
{
    public class RoomType
    {
        public string Name { get; set; }
        public List<string> Amenities { get; set; }
        public int MaxOccupancy { get; set; }
    }
}
