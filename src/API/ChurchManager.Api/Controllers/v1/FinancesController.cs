using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class FinancesController : BaseApiController
    {
        private readonly ILogger<FinancesController> _logger;
        private readonly IAppCurrentUser _currentUser;

        public FinancesController(
            ILogger<FinancesController> logger,
            IAppCurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }
        
        
    }
}
