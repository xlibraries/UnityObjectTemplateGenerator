using System.Collections.Generic;

[System.Serializable]
public class JsonDataList
{
    public List<JsonData> data;

    public int Count { get; internal set; }
}
