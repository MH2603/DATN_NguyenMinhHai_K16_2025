using System.Collections.Generic;
using System.Linq;

namespace MH.DataSystem
{
    /// <summary>
    /// Generic implementation of IDataProvider that stores and provides access to data objects.
    /// </summary>
    /// <typeparam name="T">The type of data objects. Must implement IIdentifiable.</typeparam>
    /// <remarks>
    /// DataProvider is responsible for:
    /// - Loading data using an IDataLoader
    /// - Deserializing data using an IDataSerializer
    /// - Storing the data in an efficient lookup structure
    /// - Providing access to the data through the IDataProvider interface
    /// </remarks>
    public class DataProvider<T> : IDataProvider<T> where T : IIdentifiable
    {
        /// <summary>
        /// Internal storage for the data objects, using their IDs as keys.
        /// </summary>
        private Dictionary<string, T> _data = new();

        /// <summary>
        /// Initializes a new instance of DataProvider with data loaded from the specified path.
        /// </summary>
        /// <param name="path">The path where the data can be found.</param>
        /// <param name="loader">The loader to use for loading the raw data.</param>
        /// <param name="serializer">The serializer to use for converting the raw data into objects.</param>
        public DataProvider(string path, IDataLoader<T> loader)
        {

            // Deserialize into objects
            IEnumerable<T> items = loader.Load(path);
            
            // Store in dictionary for fast access by ID
            foreach (var item in items)
            {
                _data[item.Id] = item;
            }
        }

        /// <summary>
        /// Gets the data object with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the data object to retrieve.</param>
        /// <returns>The data object if found; otherwise default(T).</returns>
        public T GetById(string id) => _data.TryGetValue(id, out var item) ? item : default;
        
        /// <summary>
        /// Gets all available data objects.
        /// </summary>
        /// <returns>A read-only list of all data objects.</returns>
        public IEnumerable<T> GetAll() => _data.Values.ToList();
        
        /// <summary>
        /// Checks if a data object with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if an object with the ID exists; otherwise false.</returns>
        public bool Exists(string id) => _data.ContainsKey(id);
        
        /// <summary>
        /// Gets the number of data objects available.
        /// </summary>
        public int Count => _data.Count;
    }
}
