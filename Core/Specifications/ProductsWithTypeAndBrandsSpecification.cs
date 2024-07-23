using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypeAndBrandsSpecification : BaseSpecification<Product>
    {   
        public ProductsWithTypeAndBrandsSpecification(ProductSpecParams productParams) 
            : base(x => (true) && 
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) && 
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
            // Default criteria
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand); 
            ApplyPaging(productParams.PageSize * (productParams.PageIndex -1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price); 
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p=>p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;

                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
        } 

        public ProductsWithTypeAndBrandsSpecification(int id) : base(x=> x.Id == id)
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
        }
    }
}