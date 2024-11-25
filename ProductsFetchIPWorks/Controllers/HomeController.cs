using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsFetchIPWorks.Models;
using ProductsFetchIPWorks.Services;
using ProductsFetchIPWorks.ViewModels;

namespace ProductsFetchIPWorks.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IProductService _productService;
    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var productsResponse = await _productService.GetAllProductsAsync();

        var productDetailsVm = productsResponse.Data.Select(p => new ProductDetailsViewModel
        {
            Id = p.Id,
            Title = p.Title
        }).ToList();

        var productsVm = new ProductsViewModel
        {
            Status = productsResponse.Status,
            Products = productDetailsVm
        };
        
        return View(productsVm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}