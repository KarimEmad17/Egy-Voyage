namespace EgyVoyageApi.DTOs
{
    public class HotelDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal rating { get; set; }
        public string cordinate { get; set; }
        public string location { get; set; }
        
        public IFormFile imagefile { get; set; }
    }
}
