using Microsoft.AspNetCore.Mvc;

namespace NerdStore.WebApp.MVC.Controllers;

public abstract class ControllerBase : Controller
{
    protected Guid ClienteId = Guid.Parse("DE13FD70-1E4A-4EB4-B944-09750690FD11");
}