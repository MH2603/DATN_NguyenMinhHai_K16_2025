using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;


[System.Serializable]
public class ItemData
{
    public int ID;
    public string Name;
    public int Level;
}

[System.Serializable]
public class ItemDataList
{
    public List<ItemData> items = new List<ItemData>();
}


public class GoogleSheetToJsonEditor : EditorWindow
{
    private string sheetURL = "https://docs.google.com/spreadsheets/d/your_sheet_id/export?format=csv";
    private string savePath = "Assets/Data/item_data.json";

    private List<ItemData> loadedData = new List<ItemData>();
    private Vector2 scroll;

    [MenuItem("Tools/Google Sheet Loader")]
    public static void ShowWindow()
    {
        GetWindow<GoogleSheetToJsonEditor>("Google Sheet Loader");
    }

    void OnGUI()
    {
        GUILayout.Label("Google Sheet → JSON", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        sheetURL = EditorGUILayout.TextField("Google Sheet CSV URL", sheetURL);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Load Data"))
        {
            LoadDataFromSheet(sheetURL);
        }

        if (GUILayout.Button("Save as JSON"))
        {
            SaveJson();
        }

        EditorGUILayout.Space();
        GUILayout.Label("Preview Data", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
        foreach (var item in loadedData)
        {
            EditorGUILayout.LabelField($"ID: {item.ID}, Name: {item.Name}, Level: {item.Level}");
        }
        EditorGUILayout.EndScrollView();
    }

    void LoadDataFromSheet(string url)
    {
        loadedData.Clear();
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest();

        while (!www.isDone) { }

        if (www.result == UnityWebRequest.Result.Success)
        {
            string csv = www.downloadHandler.text;
            ParseCSV(csv);
        }
        else
        {
            Debug.LogError("Failed to fetch CSV: " + www.error);
        }
    }

    void ParseCSV(string csv)
    {
        var lines = csv.Split('\n');

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            var parts = lines[i].Split(',');

            if (parts.Length < 3) continue;

            ItemData item = new ItemData();
            int.TryParse(parts[0], out item.ID);
            item.Name = parts[1];
            int.TryParse(parts[2], out item.Level);

            loadedData.Add(item);
        }

        Debug.Log($"Loaded {loadedData.Count} entries from sheet.");
    }

    void SaveJson()
    {
        ItemDataList list = new ItemDataList();
        list.items = loadedData;

        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(savePath, json);

        AssetDatabase.Refresh();
        Debug.Log("Saved JSON to: " + savePath);
    }
}
