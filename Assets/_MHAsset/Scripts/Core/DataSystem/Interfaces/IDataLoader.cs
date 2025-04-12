using System;
using System.Collections.Generic;

namespace MH.DataSystem
{
    /// <summary>
    /// Interface for loading raw text data from various sources.
    /// </summary>
    public interface IDataLoader<T> where T : IIdentifiable
    {
        /// <summary>
        /// Loads raw text data from a specified path.
        /// </summary>
        /// <param name="path">The path to load data from.</param>
        /// <returns>The raw text data as a string.</returns>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when the file does not exist.</exception>
        /// <exception cref="System.IO.IOException">Thrown when there's an issue reading the file.</exception>
        List<T> Load(string path);
        
        /// <summary>
        /// Checks if a file exists at the specified path.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        bool Exists(string path);
    }
} 