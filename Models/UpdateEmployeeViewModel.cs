﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace NET6CRUD.Models
{
    public class UpdateEmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public int Gender { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public int DepartmentID { get; set; }
        public List<SelectListItem> DepartmentsSelectList { get; set; }
    }
}
