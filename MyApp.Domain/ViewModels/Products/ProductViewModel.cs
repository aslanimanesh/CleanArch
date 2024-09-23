using MyApp.Domain.Models;

namespace MyApp.Domain.ViewModels.Products
{
    public class ProductViewModel 
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
