using AutoMapper;
using Goba_Store.Application.Services.Interfaces;
using Goba_Store.Application.View_Models;
using Goba_Store.DataAccess.Repository.IRepository;
using Goba_Store.Models;
using Goba_Store.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Goba_Store.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _proRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public ProductService(IProductRepository productRepository, IMapper mapper, IWebHostEnvironment environment)
        {
            _proRepo = productRepository;
            _mapper = mapper;
            _environment = environment;
        }

        public ProductViewModel GetProductById(int id)
        {
            var product = _proRepo.FirstOrDefault(p => p.Id == id, includeProperities: "Category");
            return _mapper.Map<ProductViewModel>(product);
        }

        public void CreateProduct(ProductViewModel viewModel , IFormFileCollection files)
        {
            var product = _mapper.Map<Product>(viewModel);
            product.Image = ImageHelper.UploadImage(_environment, files);
            _proRepo.Add(product);
            _proRepo.Save();
        }

        public void UpdateProduct(ProductViewModel viewModel , IFormFileCollection files)
        {
            var product = _mapper.Map<Product>(viewModel);
            if (viewModel.Image != null && files.Count() > 0)
            {
                ImageHelper.DeleteImage(_environment, _proRepo.FirstOrDefault(p => p.Id == product.Id));
                product.Image = product.Image = ImageHelper.UploadImage(_environment, files);
            }
            else
            {
                var existingProduct = _proRepo.FirstOrDefault(p => p.Id == product.Id);
                product.Image = existingProduct.Image;
            }
            _proRepo.Update(product);
            _proRepo.Save();
        }

        public void DeleteProduct(int id)
        {
            var product = _proRepo.FirstOrDefault(i => i.Id == id);
            if (product != null)
            {
                ImageHelper.DeleteImage(_environment ,product);
                _proRepo.Remove(product);
                _proRepo.Save();
            }
        }


        public IEnumerable<ProductViewModel> GetAllProducts()
        {
            var products = _mapper.Map<IEnumerable<ProductViewModel>>(_proRepo.GetAll(includeProperities:"Category"));
            return products;
        }

        public IEnumerable<SelectListItem> GetDropDownList() 
        {
            return _proRepo.GetCategoryDropDownList();
        }


        public ProductDetailsViewModel GetDetailsOfProductInCart(int id , List<ProductDetailsViewModel> ShoppingCartList)
        {
            ProductDetailsViewModel model = new ProductDetailsViewModel()
            {
                Product = _proRepo.FirstOrDefault(p => p.Id == id , includeProperities:"Category")
            };

            foreach (var item in ShoppingCartList)
            {
                if (item.Product.Id == id)
                    model.InCart = true;
            }
            return model;
        }
    }
}
