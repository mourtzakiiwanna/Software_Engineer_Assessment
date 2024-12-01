using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ArrayController : ControllerBase
{
    [HttpPost]
    public IActionResult GetSecondLargest([FromBody] RequestObj request)
    {
        if (request?.RequestArrayObj == null || !request.RequestArrayObj.Any())
        {
            return BadRequest("Request array cannot be null or empty.");
        }

        var distinctNumbers = request.RequestArrayObj
            .Distinct()
            .OrderByDescending(n => n)
            .ToList();

        if (distinctNumbers.Count < 2)
        {
            return BadRequest("Not enough distinct numbers to determine the second largest.");
        }

        return Ok(distinctNumbers[1]);
    }
}
