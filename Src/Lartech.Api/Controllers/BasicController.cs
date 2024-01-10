using Microsoft.AspNetCore.Mvc;

namespace Lartech.Api.Controllers
{
    public abstract class  BasicController : ControllerBase
    {
        protected IActionResult RetornoRequest(object? result)
        {
            if(result != null)
            {
                return Ok(new
                {
                    Sucesso = true,
                    Data = result
                });
            }
            return NotFound(new
            {
                Sucesso = false,
                Data = result
            });

        }

        protected IActionResult RetornoRequest(object? result, List<string> erros)
        {
            if (erros.Any())
            {
                return BadRequest(new
                {
                    Sucesso = false,
                    Data = result,
                    Mensagens = erros
                });
            }
            else
            {
                return RetornoRequest(result);
            }
        }

    }
}
