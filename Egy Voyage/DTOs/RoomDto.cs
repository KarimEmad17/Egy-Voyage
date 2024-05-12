using EgyVoyageApi.Entities;

namespace EgyVoyageApi.DTOs
{
    public class RoomDto
    {
        public string Name { get; set; }
        public string category { get; set; }
        public int price { get; set; }
        public int HotelId { get; set; }
       public string image {  get; set; }
        public int capacity { get; set; }
        public bool freeWifi { get; set; }=false;
        public bool smoking { get; set; }=false;
        public bool breakfast { get; set; } = false;
    }
}
