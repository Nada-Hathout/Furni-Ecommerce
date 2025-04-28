using BusinessLogic.Service;
using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Furni_Ecommerce_DashBoard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        public IActionResult Index()
        {

            List<Product> products = productService.GetAllProducts();
            var productVM = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
            }).ToList();
            return View("Index", productVM);
        }
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Stock = productViewModel.Stock,
                };

            productService.AddProduct(product);
                return RedirectToAction("Index", product);

            }
            
            return View(productViewModel);
        }
        public IActionResult Edit(int id)
        {
            var product = productService.GetProdById(id);
            if(product == null)
            {
                return NotFound();
            }
            var prodVM = new ProductViewModel
            {

                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };

            return View("Edit",prodVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id , ProductViewModel productViewModel)
        {
            if(id != productViewModel.Id)
            {
                return NotFound(productViewModel.Name);
            }
            if(ModelState.IsValid)
            {

                var product = productService.GetProdById(id);
                if(product == null)
                {
                    return NotFound();
                }
                product.Name = productViewModel.Name;
                product.Price = productViewModel.Price;
                product.Stock = productViewModel.Stock;
                productService.EditProduct(product);
                return RedirectToAction("Index", product);
            }
            return View(productViewModel);
            
          
        }
        public IActionResult Details(int id)
        {
            var product = productService.GetProdById(id);
            if (product == null)
            {
                return NotFound();
            }
            var prodVM = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };
            return View("Details", prodVM);
        }

        public IActionResult Delete(int id)
        {
            productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

    }
}
