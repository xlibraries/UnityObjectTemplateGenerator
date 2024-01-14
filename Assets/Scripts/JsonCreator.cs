using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonCreator : JsonEditor
{
    [MenuItem("Window/JsonEditor/Create JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonCreator>("Create JSON");
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        if (GUILayout.Button("Create JSON Data"))
        {
            CreateJsonData();
        }
    }

    private void CreateJsonData()
    {
        string path = EditorUtility.SaveFilePanel("Create JSON File", "", newTemplate.name, "json");
        if (path.Length != 0)
        {
            string jsonString = JsonUtility.ToJson(newTemplate);
            File.WriteAllText(path, jsonString);
        }
    }
}
