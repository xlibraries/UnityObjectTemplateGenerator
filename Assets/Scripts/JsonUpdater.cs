using UnityEditor;
using UnityEngine;

public class JsonUpdater : JsonEditor
{
    [MenuItem("Window/JsonEditor/Update JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonUpdater>("Update JSON");
    }

    protected override void OnGUI()
    {
        base.OnGUI();
        // GUI code here
        if (GUILayout.Button("Update JSON Data"))
        {
            UpdateJsonData();
        }
    }

    //Edit JSON data
    private void UpdateJsonData()
    {
        // ToDo - Update JSON data
        //CreateJsonData();
    }
}
