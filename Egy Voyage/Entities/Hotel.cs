namespace EgyVoyageApi.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string location {  get; set; }
        public string cordinate {  get; set; }
        public decimal rating { get; set; }
        public ICollection<feedback_Hotel> feedbacks { get; set; }
        public ICollection<Hotel_Image> images { get; set; }
        

        public ICollection<room> rooms { get; set; }
    }
}
