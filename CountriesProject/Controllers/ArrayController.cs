using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArrayController : ControllerBase
    {
        private readonly ArrayService _arrayService;

        public ArrayController(ArrayService arrayService)
        {
            _arrayService = arrayService;
        }

        [HttpPost]
        public IActionResult GetSecondLargest([FromBody] RequestObj request)
        {
            try
            {
                var secondLargest = _arrayService.GetSecondLargest(request);
                return Ok(secondLargest);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
