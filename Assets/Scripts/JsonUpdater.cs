using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonUpdater : JsonEditor
{
    //Edit JSON data
    public void UpdateJsonData()
    {
        string path = GetJsonFilePath();
        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            JsonDataList templateList = JsonUtility.FromJson<JsonDataList>(jsonString);

            Dictionary<string, GameObject> nameToObjectMap = new Dictionary<string, GameObject>();
            GameObject canvas = GameObject.FindObjectOfType<Canvas>()?.gameObject;

            foreach (JsonData template in templateList.data)
            {
                GameObject obj = CreateGameObject(template, ref canvas);
                nameToObjectMap[template.name] = obj;
            }

            foreach (JsonData template in templateList.data)
            {
                if (template.parent != null && nameToObjectMap.ContainsKey(template.parent))
                {
                    GameObject parentObj = nameToObjectMap[template.parent];
                    nameToObjectMap[template.name].transform.SetParent(parentObj.transform);
                }
            }
        }
    }
}
