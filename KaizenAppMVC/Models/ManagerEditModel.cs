using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KaizenAppMVC.Models
{
    public class ManagerEditModel
    {
        [Key]
        [Display(Name = "Cost Center")]
        public int DepartmentID {get; set;}
        [Display(Name ="Department")]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Morate popuniti ovo polje")]
        [Display(Name ="Menadzer (ime i prezime)")]
        public string DepartmentManager { get; set; }
        [Display(Name ="Imejl menadzera")]
        public string ManagerEmail { get; set; }
        [Display(Name ="Menadzer Username")]
        public string ManagerUsername { get; set; }
    
    }
}