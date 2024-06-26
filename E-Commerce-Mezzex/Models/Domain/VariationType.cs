using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Mezzex.Models.Domain
{
    public enum ControlType
    {
        DropdownList,
        RadioButtonList,
        Checkboxes,
        Textbox,
        MultilineTextbox,
        DatePicker,
        FileUpload,
        ColorSquares,
        ImageSquares,
        ReadOnlyCheckboxes
    }

    public class VariationType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ControlType ControlType { get; set; }

        public ICollection<VariationValue> VariationValues { get; set; } = new List<VariationValue>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
