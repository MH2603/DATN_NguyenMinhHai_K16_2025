# Unity Modular Data System - Technical Design Document (TDD)

## Overview
A lightweight, modular data system for Unity, loading JSON-based game data while adhering to all SOLID principles. It supports seamless integration with other systems (Inventory, Characters, Quests) and is easy for a single developer to implement and maintain.

## System Goals
- Load data from JSON files (StreamingAssets or Resources).
- Decoupled and modular architecture.
- SOLID-compliant code structure.
- Extendable for new data types.
- Easy integration with game systems.

## Architecture Diagram (ASCII)
```
+----------------+         +--------------------+
|   JSON Files   |         |  IDataSerializer<T>|
+----------------+         +--------------------+
        |                             |
        v                             |
+----------------+         +--------------------+
|  IDataLoader   |<--------|  JsonSerializer<T> |
+----------------+         +--------------------+
        |                             
        v                             
+----------------+         +----------------+
| DataProvider<T>| ------> |  DataRegistry  |
+----------------+         +----------------+
        |                             ^
        v                             |
+----------------+          +----------------------+
| External System| <------> |    IDataProvider<T>   |
+----------------+          +----------------------+
```

## Module Breakdown
- `IDataLoader`: Loads raw text data from files.
- `IDataSerializer<T>`: Converts JSON into typed objects.
- `IDataProvider<T>`: Public interface to get data by ID or list.
- `DataProvider<T>`: Concrete implementation that holds deserialized data.
- `DataRegistry`: Central hub to store and retrieve data providers.

## Core Interfaces
```csharp
public interface IDataLoader {
    string Load(string path);
}

public interface IDataSerializer<T> {
    List<T> Deserialize(string json);
}

public interface IDataProvider<T> {
    T GetById(string id);
    IReadOnlyList<T> GetAll();
}

public interface IIdentifiable {
    string Id { get; }
}
```

## Example Data Flow
1. Load JSON from file using `StreamingAssetsLoader`.
2. Deserialize into list of objects via `JsonSerializer<T>`.
3. Store in `DataProvider<T>`.
4. Register provider in `DataRegistry`.
5. Access data via `IDataProvider<T>`.

## Example JSON File
**/StreamingAssets/Items.json**
```json
{
  "items": [
    { "id": "item001", "name": "Health Potion", "value": 50 },
    { "id": "item002", "name": "Mana Potion", "value": 30 }
  ]
}
```

## Integration Example
```csharp
var itemProvider = DataRegistry.Get<Item>();
var potion = itemProvider.GetById("item001");
```

## SOLID Principles Summary
- **S**: Single responsibility per class (Loader, Serializer, Provider).
- **O**: New data types added without modifying existing logic.
- **L**: Interfaces replaceable by their implementations.
- **I**: Narrow interfaces with focused responsibilities.
- **D**: Dependencies injected via interfaces.

## Folder Structure
```
/Scripts/DataSystem/
├── Interfaces/
├── Core/
├── Models/
├── Editor/ (optional)
```

## Extension Plan
To add new type (e.g., Enemy):
- Create model: `Enemy.cs` (implements `IIdentifiable`)
- Create JSON file: `Enemies.json`
- Create and register `DataProvider<Enemy>`

## Unit Testing Strategy
- Loader: File read success/error cases.
- Serializer: Valid/invalid JSON.
- Provider: Fetching by ID and list.

## Optional Editor Tooling
`DataViewerWindow`: Editor tool to preview loaded data.

## Future Improvements
- Async loading support.
- Hot-reloading in Editor.
- Localization layer.
- Data validation layer.

