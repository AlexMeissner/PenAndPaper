using System.Collections.Generic;
using System.Threading.Tasks;
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

        public static async void RemoveAll<T>(this ICollection<T> collection, IList<T> items)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var item in items)
                {
                    collection.Remove(item);
                }
            });
        }

        public static async Task InvokeAddAsync<T>(this ICollection<T> collection, T item)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                collection.Add(item);
            });
        }

        public static async Task InvokeRemoveAsync<T>(this ICollection<T> collection, T item)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                collection.Remove(item);
            });
        }
    }
}
