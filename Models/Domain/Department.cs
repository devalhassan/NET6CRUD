using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET6CRUD.Models.Domain
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        [Display(Name ="ID")]
        public int DepartmentID { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        [Column(TypeName = "nvarchar(200)")]
        public string DepartmentName { get; set; }
    }
}
