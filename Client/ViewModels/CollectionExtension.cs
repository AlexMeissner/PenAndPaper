using System.Collections.Generic;
using System.Windows;

namespace Client.ViewModels
{
    public static class CollectionExtension
    {
        public static async void ReplaceWith<T>(this ICollection<T> collection, ICollection<T> other)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                collection.Clear();

                foreach (var item in other)
                {
                    collection.Add(item);
                }
            });
        }
    }
}
