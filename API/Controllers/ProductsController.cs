using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController (IGenericRepo<Product> repo): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        var spec = new ProductSpec(brand, type);
        var products = await repo.ListAsync(spec);
        
        return Ok(products);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product != null) return product;
        return NotFound("The product is not found");
    }
    
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        if (await repo.SaveChangesAsync())
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        return BadRequest("Cannot add the product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (!repo.IsExists(id) || product.Id != id) return BadRequest("Cannot update this product");

        repo.Update(product);
        
        if(await repo.SaveChangesAsync())  return NoContent();
        return BadRequest("Cannot update the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();
        
        repo.Delete(product);
        
        if(await repo.SaveChangesAsync())  return NoContent();
        return BadRequest("Cannot delete the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
       //@TODO: GetBrands with specification pattern
       return Ok();

    }
    
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        //@TODO: GetTypes with specification pattern
        return Ok();
    }
}