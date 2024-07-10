using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Models
{
    public class LoginResponse(string token) : IActionResult
    {
        private string Token => token;
        
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(Token)
            {
                StatusCode = StatusCodes.Status200OK,
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
