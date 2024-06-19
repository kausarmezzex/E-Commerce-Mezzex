namespace E_Commerce_Mezzex.Models.Domain
{
    public enum MediaType
    {
        Image,
        Video
    }

    public class Picture
    {
        public int Id { get; set; }
        public string VirtualPath { get; set; }
        public int ProductId { get; set; }
        public string SeoFilename { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public MediaType MediaType { get; set; }
        public Product Product { get; set; }
    }
}
