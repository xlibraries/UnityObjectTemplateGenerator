//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//public class JsonLoader : JsonEditor
//{
//    protected List<JsonData> newTemplates = new List<JsonData>();
//    protected override void OnGUI()
//    {
//        //base.OnGUI();

//        if (GUILayout.Button("Load JSON Data"))
//        {
//            LoadJsonData();
//        }
//    }

//    public void LoadJsonData()
//    {
//        // Deserialize JSON to List<JsonData>
//        string path = GetJsonFilePath();
//        string json = File.ReadAllText(path);
//        List<JsonData> loadedTemplates = JsonUtility.FromJson<List<JsonData>>(json);

//        // Clear the current templates
//        newTemplates.Clear();

//        // Add loaded templates to newTemplates
//        foreach (JsonData template in loadedTemplates)
//        {
//            newTemplates.Add(template);
//        }
//    }
//}

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

public class JsonLoader : JsonEditor
{
    protected List<JsonData> newTemplates = new List<JsonData>();
    private string[] options = new string[] { "Local", "Remote" };
    private int index = 0;
    private string serverUrl = "";

    protected override void OnGUI()
    {
        //base.OnGUI();

        index = EditorGUILayout.Popup(index, options);
        if (options[index] == "Remote")
        {
            serverUrl = EditorGUILayout.TextField("Server URL", serverUrl);
        }

        if (GUILayout.Button("Load JSON Data"))
        {
            if (options[index] == "Local")
            {
                LoadJsonData();
            }
            else if (options[index] == "Remote")
            {
                FetchTemplates();
            }
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

    [System.Obsolete]
    public async void FetchTemplates()
    {
        string url = serverUrl;

        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON to List<JsonData>
            List<JsonData> loadedTemplates = JsonUtility.FromJson<List<JsonData>>(json);

            // Clear the current templates
            newTemplates.Clear();

            // Add loaded templates to newTemplates
            foreach (JsonData template in loadedTemplates)
            {
                newTemplates.Add(template);
            }
        }
        else
        {
            Debug.Log($"Error: {response.StatusCode}");
        }
    }
}
