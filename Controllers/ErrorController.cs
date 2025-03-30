using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // <--- Agrega este atributo
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            // Aquí puedes manejar o registrar la excepción como desees
            return Problem(detail: exception?.Message, title: "An unexpected error occurred.");
        }
    }
}
