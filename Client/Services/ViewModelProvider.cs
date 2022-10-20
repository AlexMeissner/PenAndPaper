using System.Windows.Controls;

namespace Client.Services
{
    public interface IViewModelProvider
    {
        public void Register<T>(UserControl control);
    }

    public class ViewModelProvider : IViewModelProvider
    {
        public void Register<T>(UserControl control)
        {
            throw new System.NotImplementedException();
        }
    }
}