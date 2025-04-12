# Unity Modular Data System - Technical Design Document

## 1. Overview

MH Data System is a lightweight, modular data management solution for Unity projects that allows loading and accessing game data from JSON files. The system follows SOLID principles with a focus on modularity, extensibility, and maintainability.

## 2. System Architecture

### 2.1 Core Components

```
+----------------+         +----------------+
|   JSON Files   |         |  IIdentifiable |
+----------------+         +----------------+
        |                          ^
        v                          |
+----------------+         +----------------+
| IDataLoader<T> |-------->|   Data Model   |
+----------------+         +----------------+
        |                          ^
        v                          |
+----------------+         +----------------+
| DataProvider<T>|-------->| IDataProvider<T>|
+----------------+         +----------------+
        ^                          ^
        |                          |
+----------------+         +----------------+
| External System|<--------|  Game Systems  |
+----------------+         +----------------+
```

### 2.2 Key Interfaces and Classes

#### 2.2.1 IIdentifiable
The foundation interface that all data models must implement to be used with the data system.
```csharp
public interface IIdentifiable 
{
    string Id { get; }
}
```

#### 2.2.2 IDataLoader<T>
Responsible for loading data of type T from various sources.
```csharp
public interface IDataLoader<T> where T : IIdentifiable
{
    List<T> Load(string path);
    bool Exists(string path);
}
```

#### 2.2.3 IDataProvider<T>
The primary interface for accessing data objects used by external systems.
```csharp
public interface IDataProvider<T> where T : IIdentifiable
{
    T GetById(string id);
    IEnumerable<T> GetAll();
    bool Exists(string id);
    int Count { get; }
}
```

#### 2.2.4 DataProvider<T>
The concrete implementation of IDataProvider<T> that stores and provides access to data.
```csharp
public class DataProvider<T> : IDataProvider<T> where T : IIdentifiable
{
    public DataProvider(string path, IDataLoader<T> loader);
    public T GetById(string id);
    public IEnumerable<T> GetAll();
    public bool Exists(string id);
    public int Count { get; }
}
```

#### 2.2.5 JsonLoader<T>
Implementation of IDataLoader<T> that loads data from JSON files.
```csharp
public class JsonLoader<T> : IDataLoader<T> where T : IIdentifiable
{
    public List<T> Load(string fileName);
    public bool Exists(string path);
}
```

## 3. Data Flow

1. **Data Source Definition**: Data is defined in JSON files stored in the `_MHAsset/Json/` directory.
2. **Loading**: JsonLoader<T> reads the JSON file and deserializes it into a List<T> using Unity's JsonUtility.
3. **Storage**: DataProvider<T> receives the deserialized objects and stores them in a dictionary for efficient lookup by ID.
4. **Access**: External systems access data through the IDataProvider<T> interface using GetById() or GetAll() methods.

## 4. JSON Format

The system expects JSON files to contain an array of objects, each with at least an "ID" property:

```json
[
  {
    "ID": "item001",
    "Name": "Health Potion",
    "Stats": [10, 0, 0],
    "RequireLevel": 1
  },
  {
    "ID": "item002",
    "Name": "Mana Potion",
    "Stats": [0, 10, 0],
    "RequireLevel": 1
  }
]
```

## 5. Usage Example

```csharp
// Define a data model
[System.Serializable]
public class Item : IIdentifiable
{
    public string ID;
    public string Name;
    public float[] Stats;
    public int RequireLevel;
    public string Id { get => ID; }
}

// Load and use data
void LoadData()
{
    var loader = new JsonLoader<Item>();
    var itemProvider = new DataProvider<Item>("Items.json", loader);
    
    // Get a specific item
    Item healthPotion = itemProvider.GetById("item001");
    
    // Get all items
    IEnumerable<Item> allItems = itemProvider.GetAll();
}
```

## 6. SOLID Principles Implementation

1. **Single Responsibility Principle**: Each class has a single, well-defined responsibility:
   - IDataLoader<T>: Loading data from a source
   - DataProvider<T>: Storing and providing access to data
   - Data Models: Representing specific data structures

2. **Open/Closed Principle**: The system is open for extension but closed for modification:
   - New data types can be added by implementing IIdentifiable
   - New loaders can be added by implementing IDataLoader<T>

3. **Liskov Substitution Principle**: Any implementation of an interface can be used wherever the interface is expected:
   - Any IDataLoader<T> can be used with DataProvider<T>
   - Any implementation of IDataProvider<T> can be used by external systems

4. **Interface Segregation Principle**: Interfaces are focused and minimal:
   - IIdentifiable only requires an Id property
   - IDataLoader<T> only defines methods for loading data
   - IDataProvider<T> only defines methods for accessing data

5. **Dependency Inversion Principle**: High-level modules depend on abstractions:
   - DataProvider<T> depends on IDataLoader<T>, not concrete implementations
   - External systems depend on IDataProvider<T>, not concrete implementations

## 7. Extension Guidelines

### 7.1 Adding a New Data Type

1. Create a new class implementing IIdentifiable:
```csharp
[System.Serializable]
public class Enemy : IIdentifiable
{
    public string ID;
    public string Name;
    public int Health;
    public string Id { get => ID; }
}
```

2. Create a JSON file with data for the new type.
3. Create a DataProvider<Enemy> instance with a JsonLoader<Enemy>.

### 7.2 Adding a New Loader Type

Implement the IDataLoader<T> interface for the new source type:
```csharp
public class ResourcesLoader<T> : IDataLoader<T> where T : IIdentifiable
{
    public List<T> Load(string path)
    {
        // Implementation using Resources.Load
    }
    
    public bool Exists(string path)
    {
        // Check if resource exists
    }
}
```

## 8. Future Improvements

1. **Asynchronous Loading**: Implement async loading for large data sets.
2. **DataRegistry**: Add a central registry for accessing all data providers.
3. **Validation**: Add data validation when loading.
4. **Editor Tools**: Create custom editors for managing JSON data files.
5. **Caching**: Implement smarter caching strategies for frequently accessed data.
6. **Error Handling**: Enhance error handling and reporting.
7. **Performance Optimization**: Profile and optimize data loading and access patterns.

## 9. Conclusion

The MH Data System provides a flexible, extensible foundation for managing game data in Unity projects. By following SOLID principles and using a modular approach, the system can easily evolve to meet changing requirements while maintaining a clean, maintainable codebase. 