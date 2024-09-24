using MyApp.Domain.Models;

namespace MyApp.Domain.ViewModels.Products
{
    public class EditProductViewModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageName { get; set; }
    }
}
