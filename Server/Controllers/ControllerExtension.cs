using DataTransfer;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    internal static class ControllerExtension
    {
        private static void PostProcessResponse(ApiResponse response, HttpContext content)
        {
            if (response.Error is not null)
            {
                response.Error.TraceIdentifier = content.TraceIdentifier;
            }
        }

        public static ActionResult<ApiResponse> SendResponse(this ControllerBase controller, ApiResponse response)
        {
            PostProcessResponse(response, controller.HttpContext);
            return response.Error is null ? controller.Ok(response) : controller.BadRequest(response);
        }

        public static ActionResult<ApiResponse<T>> SendResponse<T>(this ControllerBase controller, ApiResponse response)
        {
            PostProcessResponse(response, controller.HttpContext);
            return response.Error is null ? controller.Ok(response) : controller.BadRequest(response);
        }
    }
}