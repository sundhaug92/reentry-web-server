using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Checklist
{
    public string ProgramName { get; set; }
    public string ProjectName { get; set; }
    public string Filename
    {
        get
        {
            return Environment.GetEnvironmentVariable("LOCALAPPDATA") + "Low\\Wilhelmsen Studios\\ReEntry\\Checklists\\" + ProgramName + "\\" + ProjectName + "\\checklist.json";
        }
    }
    public JsonElement GetObj()
    {
        Console.WriteLine("getObj {0}", Filename);
        return JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(Filename));
    }
}