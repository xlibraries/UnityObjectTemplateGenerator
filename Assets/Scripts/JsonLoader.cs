using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonLoader : JsonEditor
{
    protected List<JsonData> newTemplates = new List<JsonData>();
    protected override void OnGUI()
    {
        //base.OnGUI();

        if (GUILayout.Button("Load JSON Data"))
        {
            LoadJsonData();
        }
    }

    public void LoadJsonData()
    {
        // Deserialize JSON to List<JsonData>
        string path = GetJsonFilePath();
        string json = File.ReadAllText(path);
        List<JsonData> loadedTemplates = JsonUtility.FromJson<List<JsonData>>(json);

        // Clear the current templates
        newTemplates.Clear();

        // Add loaded templates to newTemplates
        foreach (JsonData template in loadedTemplates)
        {
            newTemplates.Add(template);
        }
    }
}
