using Microsoft.JSInterop;
using static Website.Services.ServiceExtension;

namespace Website.Services
{
    public interface IWebGl
    {
        Task<bool> Initialize();
        Task Render();
    }

    [TransistentService]
    public class WebGl(IJSRuntime jsRuntime) : IWebGl
    {
        private IJSObjectReference? gl;

        public async Task<bool> Initialize()
        {
            try
            {
                gl = await jsRuntime.InvokeAsync<IJSObjectReference>("Initialize", "renderCanvas");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }

            return true;
        }

        // ToDo: Check if I need to clean up the webgl context and make this disposable

        public async Task Render()
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("Render", gl);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
