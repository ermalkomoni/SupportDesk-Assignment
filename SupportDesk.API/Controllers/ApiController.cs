using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace SupportDesk.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
}
