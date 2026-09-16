using System;
using System.IO;
using UnityEngine;

public class SaveService
{
    private readonly string _path;

    public SaveService()
    {
        _path = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log($"Save path: {_path}");
    }

    public SaveData Load()
    {
        try
        {
            if (!File.Exists(_path)) return new SaveData();

            string json = File.ReadAllText(_path);
            var data = JsonUtility.FromJson<SaveData>(json);
            return data ?? new SaveData();
        }
        catch (Exception e)
        {
            Debug.LogError($"Save load failed: {e.Message}");
            return new SaveData();
        }
    }

    public void Save(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_path, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }
}