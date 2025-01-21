using Microsoft.AspNetCore.Mvc; // Per ApiController, ControllerBase, HttpGet
using OrderService.Repository; // Per OrderDbContext
using System; // Per Exception
using Microsoft.EntityFrameworkCore; // Necessario per accedere a Database.GetDbConnection()

[Route("api/test")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly OrderDbContext _dbContext;

    public TestController(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult TestConnection()
    {
        try
{
    // Apri una connessione diretta al database
    var connection = _dbContext.Database.GetDbConnection();
    connection.Open(); // Apri la connessione
    connection.Close(); // Chiudi la connessione

    return Ok("Connection successful!");
}
catch (Exception ex)
{
    return StatusCode(500, $"Connection failed: {ex.Message}");
}

    }
}
