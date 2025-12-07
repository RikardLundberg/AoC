namespace AoC.Common.Extensions
{
    internal static class ListExtensions
    {
        public static T AddIfNotExists<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
            return item;
        }
    }
}
