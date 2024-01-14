using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonCreator : JsonEditor
{
    protected List<JsonData> newTemplates = new List<JsonData>();
    protected int currentTemplateIndex = 0;
    protected Vector2 scrollPosition;

    [MenuItem("Window/JsonEditor/Create JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonCreator>("Create JSON");
    }

    #region Old ONGUI
    //protected override void OnGUI()
    //{
    //    if (newTemplates.Count == 0)
    //    {
    //        newTemplates.Add(new JsonData());
    //    }

    //    GUILayout.Label("JSON Editor", EditorStyles.boldLabel);
    //    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

    //    for (int i = 0; i < newTemplates.Count; i++)
    //    {
    //        JsonData currentTemplate = newTemplates[i];

    //        // Add a dark light color shade to separate entries
    //        GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
    //        EditorGUILayout.BeginVertical("box");
    //        GUI.backgroundColor = Color.white;

    //        GUILayout.BeginHorizontal();
    //        currentTemplate.name = EditorGUILayout.TextField("Name", currentTemplate.name);
    //        if (GUILayout.Button("Delete This Entry"))
    //        {
    //            newTemplates.RemoveAt(i);
    //            break;
    //        }
    //        GUILayout.EndHorizontal();
    //        currentTemplate.parent = EditorGUILayout.TextField("Parent", currentTemplate.parent);
    //        currentTemplate.position = EditorGUILayout.Vector3Field("Position", currentTemplate.position);
    //        currentTemplate.rotation = EditorGUILayout.Vector3Field("Rotation", currentTemplate.rotation);
    //        currentTemplate.scale = EditorGUILayout.Vector3Field("Scale", currentTemplate.scale);
    //        currentTemplate.color = EditorGUILayout.ColorField("Color", currentTemplate.color);

    //        EditorGUILayout.EndVertical();
    //        EditorGUILayout.Space();
    //    }

    //    GUILayout.EndScrollView();

    //    GUILayout.BeginHorizontal();
    //    if (GUILayout.Button("Add New Entry"))
    //    {
    //        newTemplates.Add(new JsonData());
    //    }

    //    if (GUILayout.Button("Add Child"))
    //    {
    //        if (currentTemplateIndex < 3)
    //        {
    //            JsonData child = new JsonData { parent = newTemplates[currentTemplateIndex].name };
    //            newTemplates.Add(child);
    //            currentTemplateIndex++;
    //        }
    //        else
    //        {
    //            Debug.Log("Cannot add more than 3 levels of hierarchy.");
    //        }
    //    }

    //    if (GUILayout.Button("Add Parent"))
    //    {
    //        JsonData parent = new JsonData();
    //        newTemplates.Insert(0, parent);
    //        currentTemplateIndex = 0;
    //    }
    //    GUILayout.EndHorizontal();

    //    if (GUILayout.Button("Create JSON Data"))
    //    {
    //        CreateJsonData();
    //    }
    //}
    #endregion
    protected override void OnGUI()
    {
        if (newTemplates.Count == 0)
        {
            newTemplates.Add(new JsonData());
        }

        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        bool entryDeleted = false;

        for (int i = 0; i < newTemplates.Count; i++)
        {
            JsonData currentTemplate = newTemplates[i];

            // Add a dark light color shade to separate entries
            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            EditorGUILayout.BeginVertical("box");
            GUI.backgroundColor = Color.white;

            GUILayout.Label($"Entry {i + 1}", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            currentTemplate.name = EditorGUILayout.TextField("Name", currentTemplate.name);
            if (GUILayout.Button("Delete This Entry"))
            {
                newTemplates.RemoveAt(i);
                entryDeleted = true;
            }
            GUILayout.EndHorizontal();

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

        GUILayout.Label("Actions", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New Entry"))
        {
            newTemplates.Add(new JsonData());
        }

        if (GUILayout.Button("Add Child"))
        {
            if (currentTemplateIndex < 3)
            {
                JsonData child = new JsonData { parent = newTemplates[currentTemplateIndex].name };
                newTemplates.Add(child);
                currentTemplateIndex++;
            }
            else
            {
                Debug.Log("Cannot add more than 3 levels of hierarchy.");
            }
        }

        if (GUILayout.Button("Add Parent"))
        {
            JsonData parent = new JsonData();
            newTemplates.Insert(0, parent);
            currentTemplateIndex = 0;
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create JSON Data"))
        {
            CreateJsonData();
        }
    }

    private void CreateJsonData()
    {
        string path = EditorUtility.SaveFilePanel("Create JSON File", "", "", "json");
        if (path.Length != 0)
        {
            JsonDataList jsonDataList = new JsonDataList { data = newTemplates };
            string jsonString = JsonUtility.ToJson(jsonDataList);
            File.WriteAllText(path, jsonString);
        }
    }
}
