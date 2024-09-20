namespace RestaurangFrontend.Models
{
    public class Booking
    {
        public int CustomerId { get; set; }
        public int BookingId { get; set; }
        public int TableId { get; set; }
        public int TableAmount { get; set; }
        public DateTime TimeToArrive { get; set; }
    }
}
