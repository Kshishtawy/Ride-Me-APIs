namespace RideMe.Dtos
{
    public class RequestRideDto
    {
        public int? PassengerId { get; set; }

        public int? DriverId { get; set; }

        public string? RideSource { get; set; }

        public string? RideDistention { get; set; }

        public double? Price { get; set; }

    }
}
