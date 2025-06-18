namespace ChurchManager.SharedKernel.Extensions
{
    public static class ExtendedLinq
    {
        /// <summary>
        /// Determines whether the source sequence contains all elements from the values sequence.
        /// </summary>
        /// <c>true</c> if all elements in the values sequence are present in the source sequence; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> values)
        {
            return values.All(source.Contains);
        }
        public static bool ContainsAny<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> values)
        {
            return source.Any(values.Contains);
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(
                this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
        }
    }
}
