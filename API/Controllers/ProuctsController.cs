using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.Errors;
using API.Helpers;
// using System;
// using System.Collections.Generic;
// using System.Linq;

namespace API.Controllers
{
    
    public class ProductsController : BaseApiController
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
        public async Task<ActionResult<Pagination<ProductsToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {

            //var products = await _repository.GetProductsAsync(); // this is the repository method
            // var products = await _productRepo.ListAllAsync(); // this is the only list Generic Repository

            var spec = new ProductsWithTypeAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productRepo.CountAsync(countSpec);

            var products = await _productRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductsToReturnDto>>(products);
            
            return Ok(new Pagination<ProductsToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductsToReturnDto>> GetProduct(int id)
        {

            // return await _repository.GetProductsByIdAsync(id); // this is the repository method
            // return await _productRepo.GetByIdAsync(id); // this is done with the repository method

            var spec = new ProductsWithTypeAndBrandsSpecification(id); //this is being done with Generic Repo with specification

            var product = await _productRepo.GetEntityWithSpec(spec);

            if (product == null)
            {
                return NotFound(new ApiResponse(404));
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