using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonLoader : JsonEditor
{
    private JsonDataList templateList;
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
            templateList = JsonUtility.FromJson<JsonDataList>(jsonString);
            OnDrawGizmos();
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && templateList != null)
        {
            foreach (JsonData template in templateList.data)
            {
                GameObject obj = CreateGameObject(template);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(obj.transform.position, 1);
            }
        }
    }

    private GameObject CreateGameObject(JsonData template)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = template.name;
        obj.transform.position = template.position;
        obj.transform.rotation = Quaternion.Euler(template.rotation);
        obj.transform.localScale = template.scale;
        obj.GetComponent<Renderer>().material.color = template.color;
        return obj;
    }
}
