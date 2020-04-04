using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KaizenAppMVC.Models
{
    public class KaizenPregled
    {
        [Key]
        [Display(Name ="ID")]
        public int IdeaID { get; set; }
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }
        [Display(Name = "Naslov ideje")]
        public string KaizenSuggestion { get; set; }
        [Display(Name ="Opis ideje")]
        public string KaizenSolution { get; set; }
        public string Status { get; set; }
        [Display(Name ="Vreme podnošenja")]
        public string DateTime { get; set; }
        [Display(Name ="Vreme odobravanja")]
        public string DateOfApproval { get; set; }
        [Display(Name ="Korisničko ime")]
        public string UserName { get; set; }
        public string Search { get; set; }
    }
}