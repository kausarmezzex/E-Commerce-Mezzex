namespace E_Commerce_Mezzex.Models.ViewModel
{
    public class CategorySettingsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ShowOnHomePage { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public bool IncludeInTopMenu { get; set; }
    }

}
