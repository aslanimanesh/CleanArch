namespace MyApp.Domain.ViewModels.Order

{
    public class ShowOrderViewModel
    {
        public int OrderDetailId { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }

    }
}
