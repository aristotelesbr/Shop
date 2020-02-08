using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
  [Route("v1/users")]

  public class UserController : Controller
  {
    [HttpGet]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult> List([FromServices] DataContext context)
    {
      List<User> users = await context.Users
        .AsNoTracking()
        .ToListAsync();
      return Ok(users);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<User>> Create(
      [FromServices] DataContext context, [FromBody] User model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Users.Add(model);
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível criar o usuário." });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<User>> Update(
      [FromServices] DataContext context, int id, [FromBody] User model
    )
    {
      if (id != model.Id)
      {
        return NotFound(new { message = "Usuário não encontrado." });
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        context.Entry<User>(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Este registro já foi atualizado." });
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Houve um erro. POr favor tente novamente mais tarde." });
      }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> Login(
      [FromServices] DataContext context, [FromBody] User model)
    {
      var user = await context.Users
        .AsNoTracking()
        .Where(x => x.Username == model.Username && x.Password == model.Password)
        .FirstOrDefaultAsync();

      if (user == null)
        return NotFound(new { message = "Usuário ou senha inválidos." });

      var token = TokenServices.GenerateToken(user);
      return new
      {
        user = user,
        token = token
      };
    }
  }
}
