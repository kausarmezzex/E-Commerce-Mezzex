namespace E_Commerce_Mezzex.Models.Domain
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime QuestionDate { get; set; }
        public DateTime AnswerDate { get; set; }
    }
}
