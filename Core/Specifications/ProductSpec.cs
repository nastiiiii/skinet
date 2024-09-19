using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpec : BaseSpecification<Product>
{
    public ProductSpec(string? brand, string? type) : base(x =>
        (string.IsNullOrEmpty(brand) || x.Brand == brand) && (string.IsNullOrEmpty(type) || x.Type == type))
    { }
}