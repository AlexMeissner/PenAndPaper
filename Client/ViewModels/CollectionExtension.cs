using System.Collections.Generic;

namespace Client.ViewModels
{
    public static class CollectionExtension
    {
        public static void ReplaceWith<T>(this ICollection<T> collection, ICollection<T> other)
        {
            collection.Clear();

            foreach (var item in other)
            {
                collection.Add(item);
            }
        }
    }
}
