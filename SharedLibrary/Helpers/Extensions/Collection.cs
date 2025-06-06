namespace SharedLibrary.Helpers.Extensions
{
    /// <summary>
    /// Provides common extension methods for collections.
    /// </summary>
    public static class Collection
    {
        /// <summary>
        /// Determines whether the collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="list">The collection to check.</param>
        /// <returns>
        /// True if the collection is null or does not contain any elements; otherwise, false.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
    }
}
