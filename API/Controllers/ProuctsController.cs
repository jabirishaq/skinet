using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
// using System;
// using System.Collections.Generic;
// using System.Linq;

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
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productRepo = productRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductsToReturnDto>>> GetProducts()
        {

            //var products = await _repository.GetProductsAsync(); // this is the repository methot
            // var products = await _productRepo.ListAllAsync(); // this is the only list Generic Repository

            var spec = new ProductsWithTypeAndBrandsSpecification();
            var products = await _productRepo.ListAsync(spec);

            var productsDto = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductsToReturnDto>>(products);
            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsToReturnDto>> GetProduct(int id)
        {

            // return await _repository.GetProductsByIdAsync(id); // this is the repository method
            // return await _productRepo.GetByIdAsync(id); // this is done with the repository method

            var spec = new ProductsWithTypeAndBrandsSpecification(id); //this is being done with Generic Repo with specification

            var product = await _productRepo.GetEntityWithSpec(spec);

            if (product == null)
            {
                return NotFound();
            }

            // Debug log
            Console.WriteLine($"Product retrieved: {product.Name}");
            return _mapper.Map<Product, ProductsToReturnDto>(product);
            //new ProductsToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand?.Name ?? "Unknown Brand",
            //     ProductType = product.ProductType?.Name ?? "Unknown Type"
            // };
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {

            //var productBrands = await _repository.GetProductBrandsAsync();

            var productBrands = await _productBrandRepo.ListAllAsync();
            return Ok(productBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductTypes()
        {

            //var productTypes = await _repository.GetProductTypesAsync();
            var productTypes = await _productTypeRepo.ListAllAsync();
            return Ok(productTypes);
        }

    }
}