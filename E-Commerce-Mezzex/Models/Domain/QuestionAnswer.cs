namespace E_Commerce_Mezzex.Models.Domain
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime QuestionDate { get; set; }
        public int ProductId { get; set; } // Foreign key to Product
        public Product ? Product { get; set; }
    }
}
