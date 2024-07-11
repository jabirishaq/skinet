using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class ProductsController : ControllerBase
    { 
        // public readonly IProductRepository _repository; // used for Repository Method  
        // public ProductsController(IProductRepository productRepo){
        //     _repository =  productRepo;
        // }
        public readonly IGenericRepository<Product> _productRepo;
        public readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo){
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productRepo = productRepo;
           
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(){
           
        //    var products = await _repository.GetProductsAsync(); // this is the repository methot
            var products = await _productRepo.ListAllAsync();
            
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            
            // return await _repository.GetProductsByIdAsync(id); // this is the repository methot
            return await _productRepo.GetByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands(){
           
           //var productBrands = await _repository.GetProductBrandsAsync();

            var productBrands = await _productBrandRepo.ListAllAsync();
            return Ok(productBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductTypes(){
           
           //var productTypes = await _repository.GetProductTypesAsync();
            var productTypes = await _productTypeRepo.ListAllAsync();
            return Ok(productTypes);
        }

    }
}