using System.ComponentModel.DataAnnotations;

namespace ManualLaboratory.Models
{
    public class Request
    {
        public int Id { get; set; }
        [Range(1000000000, 9999999999, ErrorMessage = "Must be 10 integer")]
        public int NationalId { get; set; }
        //[Range(000000000, 9999999999, ErrorMessage = "Must be 10 integer")]
        public int UniversityNo { get; set; }
        public string StudentStatus { get; set; }
        public string College { get; set; }
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid Name must be 3 or greater")]
        public string FirstNameEn { get; set; }
        [StringLength(20, MinimumLength = 3)]
        public string FatherNameEn { get; set; }
        public string GrandfatherNameEn { get; set; }
        [StringLength(20, MinimumLength = 3)]
        public string FamilyNameEn { get; set; }
        public string FirstNameAr { get; set; }
        public string FatherNameAr { get; set; }
        public string GrandfatherNameAr { get; set; }
        public string FamilyNameAr { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //[RegularExpression(@"(05)+[0-9]{8}")]
        public int PhoneNo { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime BirthDate { get; set; }
        public int? MidecalfileNo { get; set; }
        public DateTime DateSelected { get; set; }

    }
}
