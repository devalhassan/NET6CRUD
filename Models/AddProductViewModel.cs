namespace NET6CRUD.Models
{
    public class AddProductViewModel
    {
        public string ProductName { get; set; }
        public List<IFormFile> Attatchments { get; set; }
    }
}
