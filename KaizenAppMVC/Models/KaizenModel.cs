using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace KaizenAppMVC.Models
{
    public class KaizenModel
    {
        //[MinLength(10)]
        
        [Required(ErrorMessage = "Morate popuniti ovo polje")]
        [Display(Name = "Naslov ideje")]
        public string KaizenSuggestion { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Opis ideje")]
        public string KaizenSolution { get; set; }
        [Required(ErrorMessage = "Morate popuniti ovo polje")]
        [Display(Name = "Department kome je namenjeno rešenje")]
        public string Department { get; set; }
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
        public int IdeaID { get; set; }
        public string UserName { get; set; }

    }
}


public enum Department
{
    Finance,
    IT,
    Logistics,
    HR,
    Quality,
    Engineering_Equipment,
    Engineering_Process,
    Engineering_Cutting_Process,
    Manufacturing_Cutting,
    Manufacturing_IP,
    Manufacturing_EB,
    Administration,
    CI,
    Facility,
    Operations,
    Maintenance,
    EHS
}