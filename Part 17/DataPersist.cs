using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataPersist : MonoBehaviour
{
    string path;

    public GameData GD = new GameData();

    private void Awake()
    {
        path = Application.persistentDataPath + "/data.json";
    }

    public bool TryLoad()
    {
        return File.Exists(path);
    }

    public void LoadData()
    {
        if (TryLoad())
        {
            string saveData = File.ReadAllText(path);

            GD = JsonUtility.FromJson<GameData>(saveData);
            GD.saveTime = File.GetLastWriteTime(path);
        }
    }

    public void SaveData()
    {
        string saveData = JsonUtility.ToJson(GD);

        File.WriteAllText(path, saveData);
    }
}