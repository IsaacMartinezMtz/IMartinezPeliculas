using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CineController : ControllerBase
    {
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Cine.GetAll();
            if (result.Correct) 
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }

        }

        [Route("GetById/{IdCine}")]
        [HttpGet]
        public IActionResult GetById(int IdCine)
        {
            ML.Result result = BL.Cine.GetById(IdCine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }

        }

        [Route("Add")]
        [HttpPost]
        public IActionResult Add(ML.Cine cine)
        {
            ML.Result result = BL.Cine.Add(cine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }

        }
        [Route("Delete/{IdCine}")]
        [HttpDelete]
        public IActionResult Delete(int IdCine)
        {
            ML.Result result = BL.Cine.Delete(IdCine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }

        }
        [Route("Update/{IdCine}")]
        [HttpPut]
        public IActionResult Update(int IdCine, [FromBody] ML.Cine cine)
        {
            cine.IdCine= IdCine;
            ML.Result result = BL.Cine.Update(cine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }

        }

    }
}
