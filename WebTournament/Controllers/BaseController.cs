using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebTournament.Presentation.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class BaseController : Controller
{
    
}