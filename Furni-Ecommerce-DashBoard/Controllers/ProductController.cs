using BusinessLogic.Service;
using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;

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

            var productsVM =productService.GetAllProductsAsViewModel();
            return View("Index", productsVM);
        }
        public IActionResult Details(int id)
        {
            var product = productService.GetProductViewModelById(id);
            return View("Details", product);
        }

        public IActionResult Create()
        {
            var product = new ProductViewModel
            {
                Categories = productService.GetAllCategories()
            };
            return View("Create",product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            string imageName = null;

            if (productViewModel.Img != null && productViewModel.Img.Length > 0)
            {
                imageName = Guid.NewGuid().ToString() + Path.GetExtension(productViewModel.Img.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    productViewModel.Img.CopyTo(stream);
                }
            }
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Stock = productViewModel.Stock,
                    Description = productViewModel.Description,
                    CategoryId = productViewModel.CatId,
                    ImagePath = imageName,

                };

                productService.AddProduct(product);
                return RedirectToAction("Index", product);

            }
            productViewModel.Categories = productService.GetAllCategories();
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
                Description = product.Description,
                CatId = product.CategoryId,
                ImgPath = product.ImagePath,
                Categories = productService.GetAllCategories()
            };

            return View("Edit",prodVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductViewModel productViewModel)
        {
            if(id != productViewModel.Id)
            {
                return NotFound(productViewModel.Name);
            }
            if (ModelState.IsValid)
            {

                var product = productService.GetProdById(id);
                if (product == null)
                {
                    return NotFound();
                }
                product.Name = productViewModel.Name;
                product.Price = productViewModel.Price;
                product.Stock = productViewModel.Stock;
                product.Description = productViewModel.Description;
                product.CategoryId = productViewModel.CatId;
               
                if (productViewModel.Img != null && productViewModel.Img.Length > 0)
                {

                    if (!string.IsNullOrEmpty(productViewModel.ImgPath))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", productViewModel.ImgPath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }


                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(productViewModel.Img.FileName);
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        productViewModel.Img.CopyTo(stream);
                    }
                    product.ImagePath = imageName;
                    Console.WriteLine("Saved image path: " + product.ImagePath);
                }


                productService.EditProduct(product);
                return RedirectToAction("Index", product);
            }
                productViewModel.Categories = productService.GetAllCategories();
            return View(productViewModel);

            }
        public IActionResult Delete(int id)
        {
            var prod = productService.GetProdById(id);
            if(prod == null)    return NotFound();
            var productVM = new ProductViewModel
            {
                Id=prod.Id,
                Name = prod.Name,
                Price = prod.Price, 
                Stock = prod.Stock,
                Description = prod.Description,
                CatId=prod.CategoryId,
                CategoryName = prod.Category != null ? prod.Category.Name : "",
                ImgPath = prod.ImagePath,


            };
            return View("Delete", productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

    }
}
