using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KaizenAppMVC.Models
{
    public class ConfirmCompleteModel
    {
        [Key]
        [Required]
        public int IdeaID { get; set; }
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Display(Name = "Naslov ideje")]
        public string KaizenSuggestion { get; set; }
        [Display(Name = "Opis ideje")]
        
        public string KaizenSolution { get; set; }
        [Required(ErrorMessage = "Morate popuniti ovo polje")]
        public string Status { get; set; }
        [Display(Name = "Vreme podnošenja")]
        public string DateTime { get; set; }
        [Display(Name = "ID radnika")]
        public string WorkerID { get; set; }
        [Display(Name = "Ime radnika")]
        public string WorkerName { get; set; }
        [Display(Name = "Prezime radnika")]
        public string WorkerLastname { get; set; }
        public string Search { get; set; }
        [Display(Name = "Komentar")]
        public string Comment { get; set; }
        [Display(Name = "Email odgovorne osobe")]
        public string EmailSentTo { get; set; }
        [Display(Name = "Vreme odobravanja")]
        public string DateOfApproval { get; set; }
        [Display(Name = "Odobrio")]
        public string ApprovedBy { get; set; }
        [Display(Name = "Korisnički nalog")]
        public string UserName { get; set; }
        [Display(Name = "Komentar CI koordinatora")]
        public string CiComment { get; set; }
        [Display(Name = "Vreme završetka")]
        public string DateCompleted { get; set; }
    }
}


public enum UserConfirm
{
    Confirmed_Completed,
    Not_Completed
}