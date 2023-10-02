using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    public static class ControllerExtension
    {
        public static ObjectResult NotModified(this ControllerBase controller, object? value)
        {
            return controller.StatusCode(304, value);
        }

        public static ObjectResult InternalServerError(this ControllerBase controller, object? value) 
        {
            return controller.StatusCode(500, value);
        }

        public static ObjectResult NotImplemented(this ControllerBase controller, object? value)
        {
            return controller.StatusCode(501, value);
        }
    }
}
