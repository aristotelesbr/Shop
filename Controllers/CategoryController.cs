using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

// /categories
[Route ("Categories")]
public class CategoryController : ControllerBase {
    // /categories
    [HttpGet]
    [Route ("")]
    public async Task<ActionResult<List<Category>>> List () {
        return new List<Category> ();
    }

    [HttpGet]
    [Route ("{id:int}")]
    public async Task<ActionResult<Category>> Show (int id) {
        return new Category ();
    }

    [HttpPost]
    [Route ("")]
    public async Task<ActionResult<Category>> Create ([FromBody] Category model) {
        if (!ModelState.IsValid) {
            return BadRequest (ModelState);
        }
        return Ok (model);
    }

    [HttpPut]
    [Route ("{id:int}")]
    public async Task<ActionResult<Category>> Update (int id, [FromBody] Category model) {
        // Check if sending ID is equal the model
        if (id != model.Id) {
            return NotFound (new { message = "Categoria não encontrada" });
        }

        // Check if valid data
        if (!ModelState.IsValid) {
            return BadRequest (ModelState);
        }

        return Ok (model);
    }

    [HttpDelete]
    [Route ("{id:int}")]
    public async Task<ActionResult<Category>> Destroy () {
        return Ok ();
    }
}
