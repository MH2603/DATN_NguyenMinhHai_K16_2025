using System;

namespace MH.DataSystem
{
    /// <summary>
    /// Interface for all data models that can be uniquely identified.
    /// All data models in the system must implement this interface to be used with data providers.
    /// </summary>
    /// <remarks>
    /// This is a fundamental interface in the data system architecture.
    /// Implementing this interface allows objects to be stored and retrieved by ID in data providers.
    /// </remarks>
    public interface IIdentifiable 
    {
        /// <summary>
        /// Gets the unique identifier for this data object.
        /// </summary>
        /// <remarks>
        /// IDs should be unique within a collection of the same object type.
        /// Typically this would be a string representation of a GUID, a numeric ID, or a meaningful natural key.
        /// </remarks>
        string Id { get; }
    }
}
