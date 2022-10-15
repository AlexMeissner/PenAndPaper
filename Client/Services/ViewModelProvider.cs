using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Client.Services
{
    public interface IViewModelProvider
    {
        public T Get<T>();
        public Dictionary<UserControl, List<Type>> GetRegistered();
        public void Register<T>(UserControl control);
    }
}