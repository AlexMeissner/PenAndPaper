using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Extensions;

public static class ControllerBaseExtensions
{
    public static StatusCodeResult StatusCode(this ControllerBase controllerBase, HttpStatusCode statusCode)
        => controllerBase.StatusCode((int)statusCode);
}