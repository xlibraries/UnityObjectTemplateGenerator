using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonUpdater : JsonEditor
{
    protected List<JsonData> existingTemplates = new List<JsonData>();
    private Vector2 scrollPosition;

    JsonCreator jsonCreator = new();

    [MenuItem("Window/JsonEditor/Update JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonUpdater>("Update JSON");
    }

    protected override void OnGUI()
    {
        if (GUILayout.Button("Load JSON Data"))
        {
            LoadJsonData();
        }

        if (existingTemplates.Count > 0)
        {
            GUILayout.Label("JSON Editor", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            bool entryDeleted = false;

            for (int i = 0; i < existingTemplates.Count; i++)
            {
                JsonData currentTemplate = existingTemplates[i];
                GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
                EditorGUILayout.BeginVertical("box");
                GUI.backgroundColor = Color.white;

                GUILayout.Label($"Entry {i + 1}", EditorStyles.boldLabel);

                GUILayout.BeginHorizontal();
                currentTemplate.gameObjectType = jsonCreator.ChooseGameObjectType();
                if (GUILayout.Button("Delete This Entry"))
                {
                    existingTemplates.RemoveAt(i);
                    entryDeleted = true;
                }
                GUILayout.EndHorizontal();

                currentTemplate.name = EditorGUILayout.TextField("Name", currentTemplate.name);
                currentTemplate.parent = EditorGUILayout.TextField("Parent", currentTemplate.parent);
                currentTemplate.position = EditorGUILayout.Vector3Field("Position", currentTemplate.position);
                currentTemplate.rotation = EditorGUILayout.Vector3Field("Rotation", currentTemplate.rotation);
                currentTemplate.scale = EditorGUILayout.Vector3Field("Scale", currentTemplate.scale);
                currentTemplate.color = EditorGUILayout.ColorField("Color", currentTemplate.color);

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                if (entryDeleted)
                {
                    break;
                }
            }

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Update JSON Data"))
            {
                jsonCreator.CreateJsonData();
            }
        }
    }

    private void LoadJsonData()
    {
        string path = GetJsonFilePath();
        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            JsonDataList jsonDataList = JsonUtility.FromJson<JsonDataList>(jsonString);
            existingTemplates = jsonDataList.data;
        }
    }

    //private void UpdateJsonData()
    //{
    //    string path = EditorUtility.SaveFilePanel("Update JSON File", "", "", "json");
    //    if (path.Length != 0)
    //    {
    //        JsonDataList jsonDataList = new JsonDataList { data = existingTemplates };
    //        string jsonString = JsonUtility.ToJson(jsonDataList);
    //        File.WriteAllText(path, jsonString);
    //    }
    //}
}
