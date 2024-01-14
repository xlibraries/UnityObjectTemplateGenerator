using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonLoader : JsonEditor
{
    [MenuItem("Window/JsonEditor/Load JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonLoader>("Load JSON");
    }

    protected override void OnGUI()
    {
        //base.OnGUI();

        if (GUILayout.Button("Load JSON Data"))
        {
            LoadJsonData();
        }
    }

    private void LoadJsonData()
    {
        string path = GetJsonFilePath();
        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            newTemplate = JsonUtility.FromJson<JsonData>(jsonString);
        }
    }
}
