using DesafioHostway.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHostway.Controllers;

[ApiController]
[Route("api/dash")]
public class AdminDashController(IDashService dashService) : ControllerBase
{

    private readonly IDashService _dashService = dashService;
}
