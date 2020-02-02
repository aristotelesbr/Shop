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
        return Ok (model);
    }

    [HttpPut]
    [Route ("{id:int}")]
    public async Task<ActionResult<Category>> Update (int id, [FromBody] Category model) {
        if (model.Id == id)
            return Ok (model);

        return NotFound ();
    }

    [HttpDelete]
    [Route ("{id:int}")]
    public async Task<ActionResult<Category>> Destroy () {
        return Ok ();
    }
}
