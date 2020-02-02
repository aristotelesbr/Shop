using Microsoft.AspNetCore.Mvc;

// /categories
[Route ("Categories")]
public class CategoryController : ControllerBase {
    // /categories
    [HttpGet]
    [Route ("")]
    public string List () {
        return "INDEX";
    }

    [HttpPost]
    [Route ("")]
    public string Create () {
        return "POST";
    }

    [HttpPut]
    [Route ("")]
    public string Update () {
        return "Update";
    }

    [HttpDelete]
    [Route ("")]
    public string Destroy () {
        return "DELETE";
    }
}
