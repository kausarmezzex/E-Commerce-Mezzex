namespace E_Commerce_Mezzex.Models.Domain
{
    public class CustomerReview
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; } // 1 to 5
        public DateTime ReviewDate { get; set; }
    }
}
