using System;
using System.Collections.Generic;

namespace MH.DataSystem
{
    /// <summary>
    /// Interface for accessing data objects. This is the primary interface that other systems
    /// will use to interact with the data system.
    /// </summary>
    /// <typeparam name="T">The type of data objects. Must implement IIdentifiable.</typeparam>
    public interface IDataProvider<T> where T : IIdentifiable
    {
        /// <summary>
        /// Retrieves an object by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the object to retrieve.</param>
        /// <returns>The object if found; otherwise null.</returns>
        T GetById(string id);
        
        /// <summary>
        /// Retrieves all available objects of type T.
        /// </summary>
        /// <returns>A collection of all available objects.</returns>
        IEnumerable<T> GetAll();
        
        /// <summary>
        /// Checks if an object with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if an object with the ID exists; otherwise false.</returns>
        bool Exists(string id);
        
        /// <summary>
        /// Gets the number of objects available.
        /// </summary>
        int Count { get; }
    }
} 