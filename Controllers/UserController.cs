using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Shop.Services;

namespace Shop.Controllers
{
  [Route("users")]
  public class UserController : Controller
  {
    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Create(
        [FromServices] DataContext context,
        [FromBody] User model)
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

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> Login(
        [FromServices] DataContext context,
        [FromBody] User model)
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