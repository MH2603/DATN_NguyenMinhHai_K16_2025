using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;

public class QuickPackageImporter : EditorWindow
{
    private AddRequest currentRequest;
    private string statusMessage = "";

    [MenuItem("Window/Quick Package Importer")]
    public static void ShowWindow()
    {
        GetWindow<QuickPackageImporter>("Quick Package Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Quick Package Importer", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // Nút import Universal RP
        if (GUILayout.Button("Import Universal RP"))
        {
            ImportPackage("com.unity.render-pipelines.universal", "Installing Universal RP...");
        }


        // Nút import TextMeshPro (ví dụ khác)
        if (GUILayout.Button("Import TextMeshPro"))
        {
            ImportPackage("com.unity.textmeshpro", "Installing TextMeshPro...");
        }

        // Thêm nút cho package từ Git URL (ví dụ)
        if (GUILayout.Button("Import UniTask"))
        {
            ImportPackage(" https://github.com/Cysharp/UniTask", "Installing UniTask Git Package...");
        }

        // Hiển thị trạng thái
        if (!string.IsNullOrEmpty(statusMessage))
        {
            EditorGUILayout.HelpBox(statusMessage, MessageType.Info);
        }
    }

    private void ImportPackage(string packageIdOrUrl, string installingMessage)
    {
        statusMessage = installingMessage;
        Repaint();

        currentRequest = Client.Add(packageIdOrUrl);
        EditorApplication.update += CheckImportProgress;
    }

    private void CheckImportProgress()
    {
        if (currentRequest != null && currentRequest.IsCompleted)
        {
            if (currentRequest.Status == StatusCode.Success)
            {
                statusMessage = "Package installed successfully!";
            }
            else if (currentRequest.Status >= StatusCode.Failure)
            {
                statusMessage = $"Failed to install package: {currentRequest.Error.message}";
                Debug.LogError(currentRequest.Error.message);
            }

            EditorApplication.update -= CheckImportProgress;
            currentRequest = null;
            Repaint();
        }
    }
}