using Microsoft.AspNetCore.Mvc.Rendering;
namespace ManualLaboratory.Models
{
    public class VMCollege
    {
        public Request request { get; set; }    
        public SelectList CollegesSelectList { get; set; }    
    }
}
