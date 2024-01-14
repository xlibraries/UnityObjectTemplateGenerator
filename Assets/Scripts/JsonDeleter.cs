using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonDeleter : JsonEditor
{
    [MenuItem("Window/JsonEditor/Delete JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonDeleter>("Delete JSON");
    }

    protected override void OnGUI()
    {
        //base.OnGUI();
        // GUI code here
        if (GUILayout.Button("Delete JSON Data"))
        {
            DeleteJsonData();
        }
    }

    //Delete JSON data
    private void DeleteJsonData()
    {
        string path = GetJsonFilePath();
        if (path.Length != 0 && File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
