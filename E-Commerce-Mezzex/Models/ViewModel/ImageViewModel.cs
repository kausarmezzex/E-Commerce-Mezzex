using E_Commerce_Mezzex.Models.Domain;

namespace E_Commerce_Mezzex.Models.ViewModel
{
    public class ImageViewModel
    {
        public string VirtualPath { get; set; }
        public int ProductId { get; set; }
        public string SeoFilename { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public MediaType MediaType { get; set; }
        public int? VariationValueId { get; set; }
    }

}
