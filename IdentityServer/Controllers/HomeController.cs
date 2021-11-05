using IdentityServer.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class HomeController : Controller
    {
        private IIdentityServerInteractionService _interactionService;

        public HomeController(IIdentityServerInteractionService interactionService) => _interactionService = interactionService;

        public async Task<IActionResult> Error(string errorId)
        {
            var errorMessage = await _interactionService.GetErrorContextAsync(errorId);
            var viewModel = new ErrorViewModel
            {
                Error = errorMessage
            };
            return View(viewModel);
        }
    }
}
