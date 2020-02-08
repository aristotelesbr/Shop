using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

// /categories
[Route("v1/categories")]
public class CategoryController : ControllerBase
{
  // /categories
  [HttpGet]
  [Route("")]
  [Authorize(Roles = "employee")]
  public async Task<ActionResult<List<Category>>> List([FromServices] DataContext context)
  {
    var categories = await context.Categories.AsNoTracking().ToListAsync();
    return Ok(categories);
  }

  [HttpGet]
  [Route("{id:int}")]
  [Authorize(Roles = "employee")]
  public async Task<ActionResult<Category>> Show(int id, [FromServices] DataContext context)
  {
    var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    return Ok(category);
  }

  [HttpPost]
  [Route("")]
  public async Task<ActionResult<Category>> Create(
    [FromBody] Category model, [FromServices] DataContext context)
  {

    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      context.Categories.Add(model);
      await context.SaveChangesAsync();
      return Ok(model);
    }
    catch
    {
      return BadRequest(new { message = "Houve um erro. Por favor tente novamente." });
    }
  }

  [HttpPut]
  [Route("{id:int}")]
  [Authorize(Roles = "employee")]
  public async Task<ActionResult<Category>> Update(
    int id, [FromBody] Category model, [FromServices] DataContext context
  )
  {
    // Check if sending ID is equal the model
    if (id != model.Id)
    {
      return NotFound(new { message = "Categoria não encontrada" });
    }

    // Check if valid data
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      context.Entry<Category>(model).State = EntityState.Modified;
      await context.SaveChangesAsync();
      return Ok(model);
    }
    catch (DbUpdateConcurrencyException)
    {
      return BadRequest(new { message = "Este registro já foi atualizado." });
    }
    catch (Exception)
    {
      return BadRequest(new { message = "Houve um erro. Por favor tente novamente." });
    }
  }

  [HttpDelete]
  [Route("{id:int}")]
  [Authorize(Roles = "employee")]
  public async Task<ActionResult<Category>> Destroy(int id, [FromServices] DataContext context)
  {
    var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    if (category == null)
    {
      return NotFound(new { message = "Categoria não encontrada." });
    }

    try
    {
      context.Categories.Remove(category);
      await context.SaveChangesAsync();
      return Ok(new { message = "Categoria removida com sucesso!" });
    }
    catch (Exception)
    {
      return BadRequest(new { message = "Não foi remover a categoria" });
    }
  }
}
