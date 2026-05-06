namespace Car_Rental_System.Models
{
    public class CarType
    {
        public int CarTypeId { get; set; }
        public string TypeName { get; set; } = "";
        public decimal DailyRate { get; set; }
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = true;

        public ICollection<Car> Cars { get; set; } = [];
    }
}
