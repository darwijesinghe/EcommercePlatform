namespace SharedLibrary.Response
{
    /// <summary>
    /// Represents the base result of an operation, containing the success status, 
    /// a descriptive message, and an optional identifier.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// A value of <c>true</c> means the operation completed as expected;
        /// <c>false</c> indicates failure or an error.
        /// </summary>
        public bool Success    { get; set; } = false;

        /// <summary>
        /// A message describing the outcome of the operation.
        /// This can be used to provide additional context such as error details 
        /// when <see cref="Success"/> is false, or confirmation notes when it’s true.
        /// </summary>
        public string? Message { get; set; } = "OK.";

        /// <summary>
        /// Optional identifier associated with the result, such as the ID of a newly created entity.
        /// </summary>
        public int Id          { get; set; }
    }

    /// <summary>
    /// Represents a standardized response for operations that return data, 
    /// extending <see cref="Result"/> to include a strongly typed payload.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the data payload returned by the operation. Must be a reference type.
    /// </typeparam>
    public class Result<T> : Result where T : class
    {
        /// <summary>
        /// The data payload returned by the operation, if applicable.
        /// When <see cref="Result.Success"/> is <c>true</c>, this typically contains the expected result;
        /// otherwise, it may be <c>null</c>.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Indicates whether the <see cref="Data"/> property contains meaningful content.
        /// Returns <c>true</c> if:
        /// <list type="bullet">
        ///   <item><description>Data is a non-empty string</description></item>
        ///   <item><description>Data is a non-empty collection (implements <see cref="System.Collections.IEnumerable"/>)</description></item>
        ///   <item><description>Data is a non-null single object</description></item>
        /// </list>
        /// Returns <c>false</c> if <see cref="Data"/> is <c>null</c>, empty, or whitespace (for strings).
        /// </summary>
        public bool HasData => HasUsefulData(Data);

        /// <summary>
        /// Determines whether the specified <paramref name="data"/> object contains meaningful content.
        /// </summary>
        /// <param name="data">
        /// The object to evaluate. This may be <c>null</c>, a string, a collection, or a reference type instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if:
        /// <list type="bullet">
        ///   <item><description><paramref name="data"/> is a non-empty string</description></item>
        ///   <item><description><paramref name="data"/> is a non-empty collection (implements <see cref="System.Collections.IEnumerable"/>)</description></item>
        ///   <item><description><paramref name="data"/> is a non-null reference type</description></item>
        /// </list>
        /// <c>false</c> if <paramref name="data"/> is <c>null</c>, an empty string, or an empty collection.
        /// </returns>
        private static bool HasUsefulData(object? data)
        {
            if (data == null)
                return false;

            if (data is string str)
                return !string.IsNullOrWhiteSpace(str);

            // checks if the object is a collection and contains at least one element
            if (data is System.Collections.IEnumerable enumerable)
                return enumerable.GetEnumerator().MoveNext();

            // fallback for non-null single objects (e.g., DTOs)
            return true;
        }
    }
}
