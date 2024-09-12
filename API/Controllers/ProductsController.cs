using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController (IProductRepo productRepo): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await productRepo.GetProductsAsync(brand, type, sort));
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepo.GetProductByIdAsync(id);
        if (product != null) return product;
        return NotFound("The product is not found");
    }
    
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepo.AddProduct(product);
        if (await productRepo.SaveChangesAsync())
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        return BadRequest("Cannot add the product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (!productRepo.ProductExists(id) || product.Id != id) return BadRequest("Cannot update this product");

        productRepo.UpdateProduct(product);
        
        if(await productRepo.SaveChangesAsync())  return NoContent();
        return BadRequest("Cannot update the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepo.GetProductByIdAsync(id);

        if (product == null) return NotFound();
        
        productRepo.DeleteProduct(product);
        
        if(await productRepo.SaveChangesAsync())  return NoContent();
        return BadRequest("Cannot delete the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await productRepo.GetBrandsAsync());
    }
    
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await productRepo.GetTypesAsync());
    }
}