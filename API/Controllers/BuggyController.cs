using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly DataContext _context;

    public BuggyController(DataContext context)
    {
        _context = context;
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // GET: api/buggy/auth
    [HttpGet("auth")]
    [Authorize]
    public ActionResult<string> GetSecret()
    {
        // para ver la respuesta de NO autorizado
        return "secret text";
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // GET: api/buggy/not-found
    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        // para respuesta de not found
        var thing = _context.Users.Find(-1);

        if (thing == null) return NotFound();

        return Ok(thing);
    }

    //////////////////////////////////////////////// 54
    ///////////////////////////////////////////////////
    // GET: api/buggy/server-error
    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        // p' error null reference exception

        // me retorna null y al aplicarle un metodo ( .ToString() )
        // da una excepcion ( null reference exception )
        var thing = _context.Users.Find(-1);

        var thingToReturn = thing.ToString();

        return thingToReturn;
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // GET: api/buggy/bad-request
    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("Bad Request");
    }
}
