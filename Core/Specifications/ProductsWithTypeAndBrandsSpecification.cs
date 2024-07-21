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
        public ProductsWithTypeAndBrandsSpecification(string? sort, int? brandId, int? typeId) : base(x => (true) && (!brandId.HasValue || x.ProductBrandId == brandId)
        && (!typeId.HasValue || x.ProductTypeId == typeId)) // Default criteria
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            //AddOrderBy(x => x.Name);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
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