using System.Collections.Generic;

namespace Repositories.Tests.DataGenerators.Base
{
    /// <summary>
    /// Interface to define the required methods in fakeDataGenerator
    /// </summary>
    /// <typeparam name="T">Domain class from the Models</typeparam>
    public interface IFakeDataGenerator<T>
    {
        /// <summary>
        /// Generates one object of the type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A single object.</returns>
        T Generate();

        /// <summary>objects of the type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="amount">The amount of objects that should be generated.</param>
        /// <returns>A collection of objects.</returns>
        IEnumerable<T> GenerateRange(int amount);
    }
}
