namespace ProductsFetchIPWorks.ViewModels;

public class ProductsViewModel
{
    public bool Status { get; set; } = false;
    public IEnumerable<ProductDetailsViewModel> Products { get; set; }
}