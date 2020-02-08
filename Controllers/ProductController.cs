using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
  [Route("v1/products")]
  public class ProductController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    public async Task<ActionResult> List([FromServices] DataContext context)
    {
      List<Product> products = await context
          .Products
          .Include(x => x.Category)
          .AsNoTracking()
          .ToListAsync();

      return Ok(products);
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Show(int id, [FromServices] DataContext context)
    {
      var category = await context
          .Products
          .Include(x => x.Category)
          .AsNoTracking()
          .Where(x => x.CategoryId == id)
          .ToListAsync();

      return Ok(category);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Create(
        [FromBody] Product model, [FromServices] DataContext context)
    {

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        context.Products.Add(model);
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch
      {
        return BadRequest(new { message = "Houve um erro. Por favor tente novamente." });
      }
    }

  }
}
