using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KaizenAppMVC.Models
{
    public class CompletedKaizenModel
    {
        //[MinLength(10)]
        //[Required(ErrorMessage ="Morate popuniti ovo polje")]
        [Display(Name = "Kaizen predlog")]
        public string KaizenSuggestion { get; set; }
        //[Required(ErrorMessage = "Morate popuniti ovo polje")]
        [Display(Name = "Kaizen rešenje")]
        public string KaizenSolution { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; }
        //[Required(ErrorMessage = "Morate popuniti ovo polje")]

        public string Status { get; set; }


        [Display(Name = "Ime radnika")]
        public string WorkerName { get; set; }
        //[Required(ErrorMessage = "Morate popuniti ovo polje")]
        [Display(Name = "ID radnika")]
        public string WorkerID { get; set; }
        //[Required(ErrorMessage = "Morate popuniti ovo polje")]
        [Display(Name = "Prezime radnika")]
        public string WorkerLastname { get; set; }
        [Key]
        public int IdeaID { get; set; }
        [Display(Name = "Vreme podnošenja")]
        public string DateTime { get; set; }
        [Display(Name = "Komentar")]
       
        public string Comment { get; set; }
        [Display(Name = "Vreme odobrenja")]
        public string DateOfApproval { get; set; }
        [Display(Name = "Korisničko ime")]
        public string UserName { get; set; }
        public string Search { get; set; }
        public string ManagerEmail { get; set; }
        public string ApprovedBy { get; set; }
        [Display(Name = "Komentar")]
        [Required(ErrorMessage = "Morate popuniti ovo polje")]
        
        public string CiComment { get; set; }
        [Display(Name = "Vreme završetka")]
        public string DateCompleted { get; set; }
        public string EmailSentTo { get; set; }
    }
}